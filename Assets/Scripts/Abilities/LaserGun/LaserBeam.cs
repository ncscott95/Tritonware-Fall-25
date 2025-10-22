using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField]
    private Attack attackComponent;
    public float beamWidth;
    private List<Hitbox> enemyHitboxes;

    void Start()
    {
        enemyHitboxes = new();
        beamWidth = gameObject.transform.lossyScale.x;
    }

    void Update()
    {
        // iterating through copy of list because list changes as enemies die from laser beam
        foreach (Hitbox hitbox in new List<Hitbox>(enemyHitboxes))
        {
            if (hitbox != null)
            {
                Debug.Log($"Hitting {hitbox.gameObject.name} for {attackComponent.damage} damage");
                hitbox.TakeDamage(attackComponent);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Hitbox hitbox = collider.gameObject.GetComponent<Hitbox>();
            enemyHitboxes.Add(hitbox);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Hitbox hitbox = collider.gameObject.GetComponent<Hitbox>();
            enemyHitboxes.Remove(hitbox);
        }
    }

    public void SetLaserBeamActive(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}
