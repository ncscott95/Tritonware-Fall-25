using UnityEngine;

public class Health : MonoBehaviour
{
    protected int health;

    public int maxHealth;
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
    }

    protected void DestroyOnDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}