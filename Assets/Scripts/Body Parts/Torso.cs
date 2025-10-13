using System.Collections.Generic;
using UnityEngine;

public class Torso : MonoBehaviour
{
    public int maxHealth;
    public float decayRateMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            Health playerHealth = collision.collider.gameObject.GetComponent<Health>();
            playerHealth.decayRateMultiplier = decayRateMultiplier;
            playerHealth.maxHealth = maxHealth;
            playerHealth.healToFull();
            Destroy(gameObject);
        }
    }

}
