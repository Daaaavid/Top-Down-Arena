using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

// at the start of the game, a pickupables spawn at the positions of the entities with this component.

[GenerateAuthoringComponent]
public struct PickUpSpawn : IComponentData
{
	[HideInInspector]public Entity myPickup; // the spawns system assigns this 
}
