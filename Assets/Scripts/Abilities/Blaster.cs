using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Blaster : Ability
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float attackCooldown;
    private float elapsed;
    private int ammo;
    [SerializeField]
    private int maxAmmo = 50;

    public override void ResetAbility()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        ShowAbilityUI();
        elapsed += Time.deltaTime;
    }

    protected override void ShowAbilityUI()
    {
        if (Player.Instance.body.leftArm.equippedAbility == AbilityType.BLASTER)
        {
            abilityUI.enabled = true;
            abilityUI.text = $"Blaster ammo: {ammo}";
        }
        else
        {
            abilityUI.enabled = false;
        }
    }

    public override void ActivateAbility()
    {
        if (elapsed >= attackCooldown && ammo >= 0)
        {
            elapsed %= attackCooldown;
            ammo--;
            Instantiate(bulletPrefab, Player.Instance.transform.position, Quaternion.identity);
        }
    }
}
