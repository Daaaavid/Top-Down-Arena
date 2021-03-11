using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

public class PickupSystem : JobComponentSystem {
	private BuildPhysicsWorld buildPhysicsWorld;
	private StepPhysicsWorld stepPhysicsWorld;

	protected override void OnCreate() {
		buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
		stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
	}

	private struct ApplicationJob : ITriggerEventsJob {
		public ComponentDataFromEntity<PickupTag> pickupGroup;
		[ReadOnly] public ComponentDataFromEntity<PlayerInputButtons> playerGroup;
		public ComponentDataFromEntity<HealthData> healthGroup;
		public ComponentDataFromEntity<GunData> gunGroup;

		public void Execute(TriggerEvent triggerEvent) {
			if (playerGroup.HasComponent(triggerEvent.EntityA)) {
				if (pickupGroup.HasComponent(triggerEvent.EntityB)) {
					PickupTag pickup = pickupGroup[triggerEvent.EntityB];
					switch (pickup.type) {
						case 0:
							HealthData health = healthGroup[triggerEvent.EntityA];
							health.health += 50;
							if (health.health > 100)
								health.health = 100;
							health.damageCoolDown = .3f;
							healthGroup[triggerEvent.EntityA] = health;
							break;
						case 1:
							GunData gun = gunGroup[triggerEvent.EntityB];
							gunGroup[triggerEvent.EntityA] = gun;
							break;
						default:
							break;
					}
					pickup.destroy = true;
					pickupGroup[triggerEvent.EntityB] = pickup;
				}
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		var applicationJob = new ApplicationJob {
			pickupGroup = GetComponentDataFromEntity<PickupTag>(),
			playerGroup = GetComponentDataFromEntity<PlayerInputButtons>(true),
			healthGroup = GetComponentDataFromEntity<HealthData>(),
			gunGroup = GetComponentDataFromEntity<GunData>()
		};

		return applicationJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
	}
}
public class DestroyPickupSystem : ComponentSystem {
	protected override void OnUpdate() {
		Entities.ForEach((Entity entity, ref PickupTag pickup) => {
			if (pickup.destroy) {
				EntityManager.DestroyEntity(entity);
			}
		});
	}
}
