using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAggroZone : MonoBehaviour
{
    public Collider2D Zone;
    public List<Enemy> EnemiesInZone = new();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform target = other.transform;
            foreach (Enemy enemy in EnemiesInZone)
            {
                if (enemy != null) enemy.SetAggro(true, target);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Enemy enemy in EnemiesInZone)
            {
                if (enemy != null) enemy.SetAggro(false);
            }
        }
    }
}
