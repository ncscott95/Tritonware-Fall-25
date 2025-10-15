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
        movementComponent.canJump = true;
        movementComponent.rb.linearVelocityY = movementComponent.moveSpeed * jetStrength;
        movementComponent.canJump = true;
    }
}