using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JetpackTorso : Torso
{
    private PlayerMovement movementComponent;
    private InputAction jumpAction;

    void Start()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    public new void ActivateAbility()
    {
        if (jumpAction.triggered)
        {
            movementComponent.rb.AddForceY(1);
            movementComponent.canJump = true;
        }
    }
}