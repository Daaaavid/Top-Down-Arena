using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct UnitInputData : IComponentData {
	public float3 direction;
	public float maxSpeed;
	public float accelerationAmount;
	public float3 mousePosition;
	public bool mousePressed;
}
