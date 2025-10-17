using UnityEngine;

public class Health : MonoBehaviour
{
    protected int health;

    public int maxHealth;
    public bool isDead
    {
        get
        {
            return health <= 0;
        }
    }
    void Start()
    {
        health = maxHealth;
    }

    public void HealToFull()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        DestroyOnDeath();
    }

    public void DestroyOnDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}