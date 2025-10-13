
using UnityEngine;

public class TorsoObject : MonoBehaviour
{
    public float decayRateMultiplier;
    public int maxHealth;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("collided with player");
            Player player = Player.Instance;
            Health health = player.healthComponent;
            health.decayRateMultiplier = decayRateMultiplier;
            health.maxHealth = maxHealth;
            health.HealToFull();
            Destroy(gameObject);
        }
    }
}