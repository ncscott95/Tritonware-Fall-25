using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health { get; private set; }

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