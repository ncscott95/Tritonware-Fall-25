using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction jetpackAction;
    public InputAction primaryAttackAction;
    public InputAction secondaryAttackAction;
    public InputAction laserGunAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        jetpackAction = InputSystem.actions.FindAction("Jetpack");
        primaryAttackAction = InputSystem.actions.FindAction("Primary Attack");
        secondaryAttackAction = InputSystem.actions.FindAction("Secondary Attack");
        laserGunAction = InputSystem.actions.FindAction("Laser Gun");

    }

    void Update()
    {
        HandleMoveAction();
        HandleJumpAction();
        HandleJetpackAction();
        HandlePrimaryAttackAction();
        // HandleSecondaryAttackAction();
        HandleLaserGunAction();
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
    }

    void HandleJetpackAction()
    {
        if (jetpackAction.IsPressed() && Player.Instance.body.torso.equippedAbility == AbilityType.JETPACK)
        {
            Player.Instance.body.torso.ActivateAbility();
        }
    }

    void HandlePrimaryAttackAction()
    {
        if (primaryAttackAction.triggered)
        {
            Player.Instance.body.leftArm.ActivateAbility();
        }
    }

    void HandleSecondaryAttackAction()
    {
        if (secondaryAttackAction.triggered)
        {
            Player.Instance.body.rightArm.ActivateAbility();
        }
    }

    void HandleLaserGunAction()
    {
        if (laserGunAction.IsPressed())
        {
            Player.Instance.abilityManager.laserGun.EnableLaserBeam();

            if (Player.Instance.body.leftArm.equippedAbility == AbilityType.LASER_GUN)
            {
                Player.Instance.body.leftArm.ActivateAbility();
            }
            else if (Player.Instance.body.rightArm.equippedAbility == AbilityType.LASER_GUN)
            {
                Player.Instance.body.rightArm.ActivateAbility();
            }
        }
        else
        {
            Player.Instance.abilityManager.laserGun.DisableLaserBeam();
        }
    }
}