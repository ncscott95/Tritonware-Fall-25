using TMPro;
using UnityEngine;

public class Jetpack : Ability
{
    private PlayerMovement movementComponent;
    [SerializeField]
    private float jetStrength;
    [SerializeField]
    private float maxJetFuel = 100;
    private float jetFuel;

    void Start()
    {
        movementComponent = Player.Instance.movementComponent;
        ResetAbility();
    }

    public override void ResetAbility()
    {
        jetFuel = maxJetFuel;
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
        if (jetFuel <= 0)
        {
            Player.Instance.baseTorso.HandleCollision(new Collision2D());
            return;
        }

        movementComponent.rb.linearVelocityY = movementComponent.moveSpeed * jetStrength;
        jetFuel -= 1;
    }
}