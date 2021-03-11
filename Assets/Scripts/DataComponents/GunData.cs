using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct GunData : IComponentData {
	public int GunType; //0 = pistol, 1 = shotgun
	public Vector3 localPosition;
	public float coolDown;
	public float coolDownTimer;
}
