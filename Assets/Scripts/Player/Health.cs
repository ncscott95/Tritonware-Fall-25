using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health;

    public int maxHealth;

    [SerializeField]
    private TMP_Text healthLabel;
    [SerializeField]
    private string healthPrefix;

    private float elapsed;
    [SerializeField]
    private int decayAmount;
    [SerializeField]
    private float baseDecayRate;
    public float decayRateMultiplier;

    private float FinalDecayRate
    {
        get => baseDecayRate * decayRateMultiplier;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= FinalDecayRate)
        {
            elapsed %= FinalDecayRate;
            DecayHealth();
        }

        DestroyOnDeath();


        healthLabel.text = $"{healthPrefix}: {health}";
    }

    public void HealToFull()
    {
        health = maxHealth;
    }

    private void DecayHealth()
    {
        health -= decayAmount;
    }

    private void DestroyOnDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
