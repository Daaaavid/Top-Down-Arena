using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

[AlwaysSynchronizeSystem] //no need to wait for this piece of code
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class MovementSystem : JobComponentSystem {
	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		float deltaTime = Time.DeltaTime;
		Entities
			.WithoutBurst()
			.ForEach((ref PhysicsVelocity _physicsVelocity, in UnitInputData data) => {
				if (Vector3.SqrMagnitude(data.direction) > 0) {
					_physicsVelocity.Linear = data.direction * data.maxSpeed;
				}
			}).Run();
		return default;
	}
}
[UpdateInGroup(typeof(PresentationSystemGroup))]
public class PlayerRotationSystem : JobComponentSystem {
	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		float deltaTime = Time.DeltaTime;
		Entities
			.WithoutBurst()
			.ForEach((ref Rotation rot, ref Translation trans, in UnitInputData data) => {
				trans.Value = new float3(trans.Value.x, 0, trans.Value.z);
				if (Vector3.SqrMagnitude(data.mousePosition - trans.Value) > 1) {
					rot.Value = Quaternion.Lerp(rot.Value, Quaternion.LookRotation(data.mousePosition - trans.Value, Vector3.up), deltaTime * 10f);
				}
			}).Run();
		return default;
	}
}

