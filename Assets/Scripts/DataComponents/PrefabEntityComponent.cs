using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabEntityComponent : IComponentData
{
	public bool spawnedPickups;
	public int spawnedEnemies;
	public Entity BulletPrefab;
	public Entity EnemyPrefab;
	public Entity healthPickup;
	public Entity gunPickup;
	public Entity shotgunPickup;
}
