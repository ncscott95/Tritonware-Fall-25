using UnityEngine;
using UnityEngine.InputSystem;

public class Jetpack : Ability
{
    private PlayerMovement movementComponent;
    private InputAction jetpackAction;

    void Start()
    {
        movementComponent = Player.Instance.movementComponent;
        jetpackAction = Player.Instance.controls.jetpackAction;
    }

    public override void ActivateAbility()
    {
        movementComponent.canJump = true;
        if (jetpackAction.triggered)
        {
            Debug.Log("Jetpack triggered");
            movementComponent.rb.AddForceY(1, ForceMode2D.Impulse);
            movementComponent.canJump = true;
        }
    }
}