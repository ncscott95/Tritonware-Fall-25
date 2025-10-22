using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public DecayingHealth healthComponent;
    public AbilityType equippedAbility = AbilityType.NONE;
    public bool isUpgraded;

    public void ActivateAbility()
    {
        if (equippedAbility != AbilityType.NONE && (healthComponent == null || !healthComponent.isDead))
        {
            Player.Instance.abilityManager.abilities[equippedAbility].ActivateAbility();
        }
    }
}

public enum BodyPartType
{
    EYES,
    TORSO,
    LEFT_ARM,
    RIGHT_ARM,
    LEGS
}