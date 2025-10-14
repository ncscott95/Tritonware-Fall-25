using UnityEngine;

public class ArmObject : BodyPartObject
{
    public BodyPartType armType = BodyPartType.LEFT_ARM;

    public override void HandleCollision(Collision2D collision)
    {
        BodyPart arm;
        if (armType == BodyPartType.LEFT_ARM)
        {
            arm = Player.Instance.body.leftArm;
        }
        else if (armType == BodyPartType.RIGHT_ARM)
        {
            arm = Player.Instance.body.rightArm;
        }
        else
        {
            return;
        }
        Health armHealth = arm.healthComponent;
        arm.equippedAbility = abilityType;
        armHealth.decayRateMultiplier = decayRateMultiplier;
        armHealth.maxHealth = maxHealth;
        armHealth.HealToFull();
    }
}