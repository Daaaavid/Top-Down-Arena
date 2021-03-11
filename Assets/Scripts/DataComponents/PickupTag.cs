using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PickupTag : IComponentData
{
	public int type; // 0 = healthpickup, 1 = gunpickup;
	public bool destroy;
}
