using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction jetpackAction;
    public InputAction primaryAttackAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        jetpackAction = InputSystem.actions.FindAction("Jetpack");
        primaryAttackAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        HandleMoveAction();
        HandleJumpAction();
    }

    void HandleMoveAction()
    {
        if (moveAction.IsPressed())
        {
            Vector2 inputVector = moveAction.ReadValue<Vector2>();
            Vector2 moveVector = inputVector.x < 0 ? Vector2.left : Vector2.right;
            Player.Instance.movementComponent.Move(moveVector);
        }
        else
        {
            Player.Instance.movementComponent.Move(Vector2.zero);
        }
    }

    void HandleJumpAction()
    {
        if (jumpAction.triggered)
        {
            Player.Instance.movementComponent.Jump();
        }
        ;
    }

    void HandlePrimaryAttackAction()
    {
        if (primaryAttackAction.triggered)
        {
        }
    }
}