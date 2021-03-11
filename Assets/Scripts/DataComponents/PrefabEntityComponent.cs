using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabEntityComponent : IComponentData
{
	// This is a singlton that holds the refrences to the entities that are being spawned.
	public bool spawnedPickups;
	public int spawnedEnemies;
	public Entity BulletPrefab;
	public Entity EnemyPrefab;
	public Entity healthPickup;
	public Entity gunPickup;
	public Entity shotgunPickup;
}
