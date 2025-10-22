using UnityEngine;

public class EnemySpawner : EntitySpawner
{
    public override void SpawnEntity()
    {
        if (entityPrefab != null)
        {
            Instantiate(entityPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("EnemySpawner: No entityPrefab assigned.");
        }
    }
}
