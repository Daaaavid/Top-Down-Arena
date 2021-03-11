using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class NPCMovementSystem : ComponentSystem {

	protected override void OnUpdate() {
		float3 playerPosition = new float3();
		Entities.ForEach((ref PlayerInputButtons buttons, ref Translation trans) => {
			playerPosition = trans.Value;
		});
		Entities.ForEach((ref UnitInputData data, ref NPCTag npc, ref Translation trans) => {
			if (data.mousePressed) {
				data.direction = Vector3.zero;
			} else if (Vector3.SqrMagnitude(npc.target - trans.Value) > 4){
				data.direction = Vector3.Normalize(npc.target - trans.Value);
			} else {
				data.direction = 0;
			}
		});
	}
}
