using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class HealthCoolDownSystem : ComponentSystem {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		Entities
			.ForEach((Entity entity, ref HealthData data, ref NPCTag npc) => {
				if(data.damageCoolDown > 0) {
					data.damageCoolDown -= deltaTime;
				}
				if (data.health <= 0) {
					GameManager.main.KillUI();
					EntityManager.DestroyEntity(entity);
				}
			});

		Entities
	.ForEach((Entity entity, ref HealthData data, ref PlayerInputButtons player, ref GunData gun, ref Translation trans) => {
	if (data.damageCoolDown > 0) {
		data.damageCoolDown -= deltaTime;
		GameManager.main.HealthUI(data.health);
	}
	if (data.health <= 0) {
		trans.Value = Vector3.zero;
		data.health = 100;
			data.damageCoolDown = 0;
			gun.GunType = 0;
			gun.coolDown = 0.3f;
			GameManager.main.RestartUI();
			Entities.ForEach((ref PrefabEntityComponent prefabs) => {
				prefabs.spawnedEnemies -= GameManager.main.kills;
				prefabs.spawnedPickups = false;
			});
		}
	});
	}
}
