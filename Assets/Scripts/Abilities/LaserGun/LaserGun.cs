using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LaserGun : Ability
{
    public float range;
    public LaserBeam laserBeam;
    [SerializeField]
    private int ammo = 500;

    void Update()
    {
        ShowAbilityUI();
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

        ammo--;

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
        // LayerMask hitMask = LayerMask.GetMask("Terrain");
        // RaycastHit2D hit = Physics2D.Raycast(Player.Instance.transform.position, direction, range, hitMask);

        // Debug.DrawLine(Player.Instance.transform.position, Player.Instance.transform.position + direction * range, Color.white);

        // Debug.Log("laser gun activated");

        // if (hit)
        // {
        //     Debug.Log(hit.collider.gameObject.name);
        // }
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