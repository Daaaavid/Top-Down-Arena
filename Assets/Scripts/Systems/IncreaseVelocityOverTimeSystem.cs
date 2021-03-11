using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;

// increases the velocity of bullets (so they don't come to a standstill)

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
