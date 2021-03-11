using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;
using Unity.Mathematics;

// spawning bullets

[AlwaysSynchronizeSystem] //no need to wait for this piece of code
public class ShootingSystem : ComponentSystem {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;
		Entities
			.ForEach((ref UnitInputData player, ref Translation trans,
			ref Rotation rot, ref GunData gunData) => {
				if (gunData.coolDownTimer > 0) {
					gunData.coolDownTimer -= deltaTime;
				} else if (player.mousePressed) {
					gunData.coolDownTimer = gunData.coolDown;
					int loopamount = 1;
					switch (gunData.GunType) {
						case 0:
							loopamount = 1;
							break;
						case 1:
							loopamount = 5;
							break;
					}
					int i = 0;
					PrefabEntityComponent prefabEntityComponent = GetSingleton<PrefabEntityComponent>();
					while (i < loopamount) {
						Entity spawnedEntity = EntityManager.Instantiate(prefabEntityComponent.BulletPrefab);
						EntityManager.SetComponentData(spawnedEntity,
							new Translation { Value = trans.Value + new float3((Quaternion)rot.Value * gunData.localPosition) });
						EntityManager.SetComponentData(spawnedEntity,
							new Rotation { Value = rot.Value });
						EntityManager.SetComponentData(spawnedEntity,
							new PhysicsVelocity { Linear = (Quaternion)rot.Value * Vector3.forward * 10 });
						i++;
					}
				}
			});
	}
}
