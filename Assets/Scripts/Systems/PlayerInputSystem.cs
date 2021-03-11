using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[AlwaysSynchronizeSystem] //no need to wait for this piece of code
[UpdateInGroup(typeof(PresentationSystemGroup))]
public class PlayerInputSystem : JobComponentSystem {
	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		float deltaTime = Time.DeltaTime;
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Entities.WithoutBurst()
			.ForEach((ref UnitInputData UnitData, ref Translation trans,
			in PhysicsVelocity _physicsVelocity, in PlayerInputButtons inputData) => {
				UnitData.direction = float3.zero;
				UnitData.direction.z += Input.GetKey(inputData.upKey) ? 1 : 0;
				UnitData.direction.z -= Input.GetKey(inputData.downKey) ? 1 : 0;
				UnitData.direction.x += Input.GetKey(inputData.rightKey) ? 1 : 0;
				UnitData.direction.x -= Input.GetKey(inputData.leftKey) ? 1 : 0;
				if (Vector3.SqrMagnitude(UnitData.direction) > 0)
					UnitData.direction = math.normalize(UnitData.direction);
				UnitData.mousePosition = new float3(mouseWorldPosition.x, 0, mouseWorldPosition.z);
				UnitData.mousePressed = Input.GetMouseButton(0);

				// moving the camera with the player (This first was in the player movement system,
				// but I moved it here because the NPC's also make use of the player movement system).
				if (Vector3.SqrMagnitude(_physicsVelocity.Linear) > 1) {
					Vector3 cameraPosition = new Vector3(trans.Value.x, 10, trans.Value.z);
					GameManager.mainCamera.position = Vector3.Lerp(GameManager.mainCamera.position, cameraPosition, deltaTime);
				};
			}).Run();
		return default;
	}
}
