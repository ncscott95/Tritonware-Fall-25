using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public abstract void ActivateAbility();
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
    DOUBLE_JUMP,
    BROOM_HIT,
}