using System;
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

    public void HealByPercentage(float percentage)
    {
        int healthGained = (int)(percentage * maxHealth);
        health = Math.Min(maxHealth, health + healthGained);
    }

    public virtual void TakeDamage(int damage)
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