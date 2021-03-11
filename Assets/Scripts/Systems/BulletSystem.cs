using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;

// handles the health damage and bullet destruction when bullets hit units.

[AlwaysSynchronizeSystem]
public class BulletSystem : JobComponentSystem {
	private BuildPhysicsWorld buildPhysicsWorld;
	private StepPhysicsWorld stepPhysicsWorld;

	protected override void OnCreate() {
		buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
		stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
	}

	private struct ApplicationJob : ICollisionEventsJob {
		public ComponentDataFromEntity<BulletTag> bulletGroup;
		public ComponentDataFromEntity<HealthData> unitGroup;

		public void Execute(CollisionEvent collisionEvent) {

			if (bulletGroup.HasComponent(collisionEvent.EntityA)) {
				if (unitGroup.HasComponent(collisionEvent.EntityB)){
					HealthData data = unitGroup[collisionEvent.EntityB];
					if(data.damageCoolDown <= 0) {
						data.damageCoolDown = 0.1f;
						data.health -= 5;
						unitGroup[collisionEvent.EntityB] = data;
						BulletTag bullet = bulletGroup[collisionEvent.EntityA];
						bullet.destroyTimer = 0;
						bulletGroup[collisionEvent.EntityA] = bullet;
					}
				}
			} else if (unitGroup.HasComponent(collisionEvent.EntityA)) {
				if (bulletGroup.HasComponent(collisionEvent.EntityB)) {
					HealthData data = unitGroup[collisionEvent.EntityA];
					if (data.damageCoolDown <= 0) {
						data.damageCoolDown = 0.1f;
						data.health -= 5;
						unitGroup[collisionEvent.EntityB] = data;
						BulletTag bullet = bulletGroup[collisionEvent.EntityB];
						bullet.destroyTimer = 0;
						bulletGroup[collisionEvent.EntityA] = bullet;
					}
				}
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		var applicationJob = new ApplicationJob {
			bulletGroup = GetComponentDataFromEntity<BulletTag>(),
			unitGroup = GetComponentDataFromEntity<HealthData>()
		};

		return applicationJob.Schedule(stepPhysicsWorld.Simulation,ref buildPhysicsWorld.PhysicsWorld, inputDeps);
	}
}

[AlwaysSynchronizeSystem]
public class BulletDestroySystem : ComponentSystem {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		Entities
			.ForEach((Entity entity, ref BulletTag data, ref Translation trans) => {
				trans.Value = new float3(trans.Value.x, 0, trans.Value.z);
				data.destroyTimer -= deltaTime;
				if (data.destroyTimer <= 0) {
					EntityManager.DestroyEntity(entity);
				}
			});
	}
}
