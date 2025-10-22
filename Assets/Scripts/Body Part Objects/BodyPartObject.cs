using TMPro;
using UnityEngine;

public abstract class BodyPartObject : MonoBehaviour
{
    public AbilityType abilityType = AbilityType.NONE;
    public float decayRateMultiplier = 1;
    public int maxHealth = 100;
    public bool isUpgraded;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            HandleCollision(collision);
            Destroy(gameObject);
        }
    }

    public abstract void HandleCollision(Collision2D collision);

    protected void InitBodyPartStats(BodyPart bodyPart)
    {
        DecayingHealth bodyPartHealth = bodyPart.healthComponent;
        bodyPart.equippedAbility = abilityType;
        bodyPartHealth.decayRateMultiplier = decayRateMultiplier;
        bodyPartHealth.maxHealth = maxHealth;
        bodyPartHealth.isUpgraded = isUpgraded;
        bodyPart.isUpgraded = isUpgraded;

        if (bodyPart.equippedAbility != AbilityType.NONE)
        {
            Player.Instance.abilityManager.abilities[bodyPart.equippedAbility].ResetAbility();
        }
        bodyPartHealth.HealToFull();
    }
}