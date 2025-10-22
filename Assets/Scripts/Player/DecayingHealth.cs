using TMPro;
using UnityEngine;

public class DecayingHealth : Health
{
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

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= FinalDecayRate && !isDead)
        {
            elapsed %= FinalDecayRate;
            TakeDamage(decayAmount);
        }
    }


}
