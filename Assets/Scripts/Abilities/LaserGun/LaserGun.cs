using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LaserGun : Ability
{
    public float range;
    public LaserBeam laserBeam;
    public int attackDamage;
    [SerializeField]
    private int maxAmmo = 500;
    [SerializeField]
    private float ammoDepletionRate = 0.25f;
    private float elapsed;
    private int ammo;

    public override void ResetAbility()
    {
        ammo = maxAmmo;
    }

    void Update()
    {
        ShowAbilityUI();
        elapsed += Time.deltaTime;
    }

    protected override void ShowAbilityUI()
    {
        if (Player.Instance.body.rightArm.equippedAbility == AbilityType.LASER_GUN)
        {
            abilityUI.enabled = true;
            abilityUI.text = $"Laser gun ammo: {ammo}";
        }
        else
        {
            abilityUI.enabled = false;
        }
    }

    public override void ActivateAbility()
    {
        if (ammo <= 0) return;

        if (elapsed >= ammoDepletionRate)
        {
            ammo--;
            elapsed %= ammoDepletionRate;
        }

        EnableLaserBeam();

        Vector2 playerPosition = Player.Instance.transform.position;

        if (Player.Instance.movementComponent.facingDirection == Direction.LEFT)
        {
            Vector2 beamStart = playerPosition + new Vector2(-0.2f, 0);
            Vector2 beamEnd = beamStart + new Vector2(-range, 0);
            laserBeam.transform.position = (beamStart + beamEnd) * 0.5f;
        }
        else
        {
            Vector2 beamStart = playerPosition + new Vector2(0.2f, 0);
            Vector2 beamEnd = beamStart + new Vector2(range, 0);
            laserBeam.transform.position = (beamStart + beamEnd) * 0.5f;
        }
    }

    public void EnableLaserBeam()
    {
        laserBeam.gameObject.SetActive(true);
    }

    public void DisableLaserBeam()
    {
        laserBeam.gameObject.SetActive(false);
    }
}