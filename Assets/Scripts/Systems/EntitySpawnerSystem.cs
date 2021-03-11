using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

//Spawns enemies in a circle and pickupables at their spawn positions.
[AlwaysSynchronizeSystem]
public class EntitySpawnerSystem : ComponentSystem {

	private Unity.Mathematics.Random random;

	protected override void OnCreate() {
		random = new Unity.Mathematics.Random(56);
	}

	protected override void OnUpdate() {

		Entities.ForEach((ref PrefabEntityComponent prefabs) => {
			if (prefabs.spawnedEnemies < 100) {
				Entity spawnedEntity = EntityManager.Instantiate(prefabs.EnemyPrefab);
				Vector3 position = new Vector3(random.NextFloat(-1, 1), 0, random.NextFloat(-1, 1)).normalized;
				position *= random.NextFloat(12, 55);
				EntityManager.SetComponentData(spawnedEntity, new Translation { Value = position });
				EntityManager.SetComponentData(spawnedEntity, new NPCTag { target = position, range = 10 });
				prefabs.spawnedEnemies++;
				if (prefabs.spawnedPickups == false) {
					prefabs.spawnedPickups = true;

					PrefabEntityComponent prefabRef = prefabs;
					Entities.ForEach((ref PickUpSpawn spawn, ref Translation trans) => {
						if(!EntityManager.Exists(spawn.myPickup)) {
							float randomNumber = random.NextFloat(0, 100);
							if(randomNumber < 40) {
								spawn.myPickup = EntityManager.Instantiate(prefabRef.healthPickup);
							} else if(randomNumber < 65) {
								spawn.myPickup = EntityManager.Instantiate(prefabRef.gunPickup);
							} else {
								spawn.myPickup = EntityManager.Instantiate(prefabRef.shotgunPickup);
							}
							EntityManager.SetComponentData(spawn.myPickup, new Translation { Value = trans.Value});
						}
					});
				}
			}
		});
	}
}
