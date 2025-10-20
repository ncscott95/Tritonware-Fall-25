using TMPro;
using UnityEngine;

public class Jetpack : Ability
{
    private PlayerMovement movementComponent;
    [SerializeField]
    private float jetStrength;
    [SerializeField]
    private float jetFuel = 100;

    void Start()
    {
        movementComponent = Player.Instance.movementComponent;
    }

    void Update()
    {
        ShowAbilityUI();
    }

    protected override void ShowAbilityUI()
    {
        if (Player.Instance.body.torso.equippedAbility == AbilityType.JETPACK)
        {
            abilityUI.enabled = true;
            abilityUI.text = $"Jet Fuel: {jetFuel}";
        }
        else
        {
            abilityUI.enabled = false;
        }
    }

    public override void ActivateAbility()
    {
        if (jetFuel <= 0) return;

        movementComponent.rb.linearVelocityY = movementComponent.moveSpeed * jetStrength;
        jetFuel -= 1;
    }
}