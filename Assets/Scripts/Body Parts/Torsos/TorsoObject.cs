
using UnityEngine;

public class TorsoObject : BodyPartObject
{


    public override void HandleCollision(Collision2D collision)
    {
        BodyPart torso = Player.Instance.body.torso;
        Health torsoHealth = torso.healthComponent;
        torso.equippedAbility = abilityType;
        torsoHealth.decayRateMultiplier = decayRateMultiplier;
        torsoHealth.maxHealth = maxHealth;
        torsoHealth.HealToFull();
    }
}