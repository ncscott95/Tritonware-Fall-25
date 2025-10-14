using UnityEngine;

public abstract class BodyPartObject : MonoBehaviour
{
    public Player player = Player.Instance;
    public AbilityType abilityType = AbilityType.NONE;
    public float decayRateMultiplier = 1;
    public int maxHealth = 100;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            HandleCollision(collision);
            Destroy(gameObject);
        }
    }

    public abstract void HandleCollision(Collision2D collision);
}