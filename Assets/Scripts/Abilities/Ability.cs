using TMPro;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text abilityUI;

    public abstract void ActivateAbility();

    protected abstract void ShowAbilityUI();

    // public abstract void ResetAbility();
}

public enum AbilityType
{
    NONE,
    JETPACK,
    EMPOWERED,
    FORCE_FIELD,
    EXPLOSION,
    LASER_GUN,
    BLASTER,
    ROCKET_LAUNCHER,
    ROCKET_PUNCH,
    GRAPPLE_HOOK,
    ELECTRO_NET,
    JUMP_BOOST,
    DOUBLE_JUMP
}