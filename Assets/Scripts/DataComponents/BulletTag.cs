using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct BulletTag : IComponentData {
	public float destroyTimer;
}
