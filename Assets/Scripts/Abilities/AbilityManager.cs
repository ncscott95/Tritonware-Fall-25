using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public Dictionary<AbilityType, Ability> abilities;
    public Jetpack jetpack;
    public Blaster blaster;
    public LaserGun laserGun;

    void Start()
    {
        abilities = new Dictionary<AbilityType, Ability>
        {
            {AbilityType.JETPACK, jetpack},
            {AbilityType.BLASTER, blaster},
            {AbilityType.LASER_GUN, laserGun}
        };
    }
}