using UnityEngine;
using UnityEngine.InputSystem;

public class Jetpack : Ability
{
    private PlayerMovement movementComponent;
    [SerializeField]
    private float jetStrength;

    void Start()
    {
        movementComponent = Player.Instance.movementComponent;
    }

    public override void ActivateAbility()
    {
        movementComponent.rb.linearVelocityY = movementComponent.moveSpeed * jetStrength;
    }
}