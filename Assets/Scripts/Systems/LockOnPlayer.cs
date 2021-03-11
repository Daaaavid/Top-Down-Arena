using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

// Raycasts for the enemies to know wether they can see the player.
// It works on gameobjects and not on entities.

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class LockOnPlayer : SystemBase
{
	private EntityQuery query;
	BeginPresentationEntityCommandBufferSystem bufferSystem;
	protected override void OnCreate() {
		bufferSystem = World.GetOrCreateSystem<BeginPresentationEntityCommandBufferSystem>();
	}
	protected override void OnUpdate() {
		// stores amount of queried entities
		int dataCount = query.CalculateEntityCount();
		// concurrent command buffer for use in parralel jobs

		LayerMask walls = 1 << 6;
		Vector3 playerPosition = new Vector3();
		Entities.ForEach((ref PlayerInputButtons buttons, ref Translation trans) => {
			playerPosition = (Vector3)trans.Value;
		}).Run();

		// Allocating native arrays to store data inbetween jobs
		NativeArray<UnityEngine.RaycastHit> results = new NativeArray<UnityEngine.RaycastHit>(dataCount, Allocator.TempJob);
		NativeArray<RaycastCommand> rayCastCommand = new NativeArray<RaycastCommand>(dataCount, Allocator.TempJob);
		NativeArray<bool> hits = new NativeArray<bool>(dataCount, Allocator.TempJob);

		// building raycast commands based on selected entities
		JobHandle jobHandle = Entities.WithStoreEntityQueryInField(ref query)
			.ForEach((Entity entity, int entityInQueryIndex,in NPCTag npc, in Translation trans) =>{
				Vector3 origin = (Vector3)trans.Value;
				Vector3 direction = playerPosition - origin;
				rayCastCommand[entityInQueryIndex] = new RaycastCommand(origin, direction,npc.range,walls);
		}).ScheduleParallel(Dependency);

		JobHandle rayCastJob = RaycastCommand.ScheduleBatch(rayCastCommand, results, dataCount, jobHandle);

		// wait till raycasts are done because they seem to need to be processed on the main thread.
		rayCastJob.Complete();
		// handling raycast results on the main thread
		for(int i = 0; i < dataCount; i++) {
			if (results[i].collider == null)
				hits[i] = false;
			else
				hits[i] = true;
		}
		// Adding components to entities
		JobHandle setValue = Entities.ForEach((Entity entity, int entityInQueryIndex, ref UnitInputData data, ref NPCTag npc, in Translation trans) => {
			if(Vector3.SqrMagnitude(playerPosition - (Vector3)trans.Value) < npc.range * npc.range && !hits[entityInQueryIndex]) {
				// player is visible and in range
				data.mousePressed = true;
				data.mousePosition = playerPosition;
				npc.target = playerPosition;
			} else {
				data.mousePressed = false;
			}
		}).ScheduleParallel(rayCastJob);

		setValue.Complete();
		// Dispose Native Arrays
		results.Dispose();
		rayCastCommand.Dispose();
		hits.Dispose();
		// Buffer system depends on job "setValue"
		bufferSystem.AddJobHandleForProducer(setValue);
	}
}
