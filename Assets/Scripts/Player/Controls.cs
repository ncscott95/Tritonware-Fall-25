using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction primaryAttackAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        primaryAttackAction = InputSystem.actions.FindAction("Attack");

    }
}