using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;

public class RotateSystem : ComponentSystem {
	[BurstCompile]
    protected override void OnUpdate() {
        Entities.ForEach((ref Rotate rotate, ref RotationEulerXYZ euler) => {
            euler.Value.y += rotate.radiansPerSecond * Time.DeltaTime;
        });
    }
}
