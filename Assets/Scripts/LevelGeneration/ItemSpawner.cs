using UnityEngine;

public class ItemSpawner : EntitySpawner
{
    [SerializeField] private float spawnProbability = 1.0f; // Probability between 0 and 1

    public override void SpawnEntity()
    {
        if (entityPrefab != null)
        {
            if (Random.value > spawnProbability)
            {
                GizmoColor = Color.gray;
                return;
            }

            Instantiate(entityPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("ItemSpawner: No entityPrefab assigned.");
        }
    }
}
