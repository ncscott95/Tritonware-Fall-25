using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health;

    public int maxHealth;

    [SerializeField]
    private TMP_Text healthText;

    private float elapsed;
    [SerializeField]
    private int decayAmount;
    [SerializeField]
    private float baseDecayRate;
    public float decayRateMultiplier;

    private float finalDecayRate
    {
        get => baseDecayRate * (baseDecayRate - decayRateMultiplier);
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
        if (elapsed >= finalDecayRate)
        {
            elapsed %= finalDecayRate;
            decayHealth();
        }


        healthText.text = $"Health: {health}";
    }

    public void healToFull()
    {
        health = maxHealth;
    }

    private void decayHealth()
    {
        health -= decayAmount;
    }
}
