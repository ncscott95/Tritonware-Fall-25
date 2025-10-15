
using UnityEngine;

public class TorsoObject : BodyPartObject
{


    public override void HandleCollision(Collision2D collision)
    {
        BodyPart torso = Player.Instance.body.torso;
        DecayingHealth torsoHealth = torso.healthComponent;
        torso.equippedAbility = abilityType;
        torsoHealth.decayRateMultiplier = decayRateMultiplier;
        torsoHealth.maxHealth = maxHealth;
        torsoHealth.HealToFull();
    }
}