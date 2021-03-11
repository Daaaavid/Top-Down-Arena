using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerInputButtons : IComponentData {
	public KeyCode upKey;
	public KeyCode downKey;
	public KeyCode leftKey;
	public KeyCode rightKey;
}
