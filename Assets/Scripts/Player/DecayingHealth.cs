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
    public BodyPartType bodyPart;
    [HideInInspector] public bool isUpgraded;

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

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            if (isUpgraded)
            {
                isUpgraded = false;
                HealToFull();
                
                switch (bodyPart)
                {
                    case BodyPartType.EYES:
                        Player.Instance.baseEyes.HandleCollision(new Collision2D());
                        break;
                    case BodyPartType.TORSO:
                        Player.Instance.baseTorso.HandleCollision(new Collision2D());
                        break;
                    case BodyPartType.RIGHT_ARM:
                        Player.Instance.baseRightArm.HandleCollision(new Collision2D());
                        break;
                    case BodyPartType.LEGS:
                        Player.Instance.baseLegs.HandleCollision(new Collision2D());
                        break;
                }
            }
        }
    }
}
