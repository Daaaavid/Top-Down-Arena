using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;

[AlwaysSynchronizeSystem]
public class IncreaseVelocityOverTimeSystem : JobComponentSystem {
	protected override JobHandle OnUpdate(JobHandle inputDeps) {
		float deltaTime = Time.DeltaTime;

		Entities.ForEach((ref PhysicsVelocity vel, in SpeedIncreaseOverTimeData data) => {
			float2 modifier = new float2(data.increasePerSecond * deltaTime);
			float2 newVel = vel.Linear.xz;

			newVel += math.lerp(-modifier, modifier, math.sign(newVel));
			vel.Linear.xz = newVel;
		}).Run();

		return default;
	}
}
