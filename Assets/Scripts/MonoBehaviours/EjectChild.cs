using UnityEngine;

[SelectionBase]
public class EjectChild : MonoBehaviour
{
	// My walls collide with the characters when they are entities but the raycast works on game objects.
	// So for now I made this script disconnecting the child from the parent so that when the wall becomes an entity
	// there is still a collider that is a gameobject.
	private void Awake() {
		foreach (Transform t in transform.GetComponentInChildren<Transform>()) {
			if(t != transform) {
				t.parent = null;
			}
		}
	}
}
