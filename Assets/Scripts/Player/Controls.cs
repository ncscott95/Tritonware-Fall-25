using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    [Header("Input Actions")]
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
        HandleSecondaryAttackAction();
        HandleLaserGunAction();
    }

    void HandleMoveAction()
    {
        if (moveAction.IsPressed())
        {
            Vector2 inputVector = moveAction.ReadValue<Vector2>();

            if (inputVector.x == 0)
            {
                Player.Instance.animator.SetBool("isWalking", false);
                return;
            }

            Vector2 moveVector = inputVector.x < 0 ? Vector2.left : Vector2.right;
            Player.Instance.movementComponent.Move(moveVector);

            // TODO: fix walking animation restarting when switching directions
            Player.Instance.animator.SetBool("isWalking", true);
        }
        else
        {
            Player.Instance.movementComponent.Move(Vector2.zero);
            Player.Instance.animator.SetBool("isWalking", false);
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
        if (Player.Instance.body.torso.equippedAbility != AbilityType.JETPACK)
        {
            Player.Instance.SetJetpackActiveVisuals(false, false);
            return;
        }

        if (jetpackAction.IsPressed())
        {
            Player.Instance.body.torso.ActivateAbility();
            Player.Instance.SetJetpackActiveVisuals(true, true);
        }
        else
        {
            Player.Instance.SetJetpackActiveVisuals(true, false);
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

            if (Player.Instance.body.leftArm.equippedAbility == AbilityType.LASER_GUN)
            {
                Player.Instance.body.leftArm.ActivateAbility();
            }
            else if (Player.Instance.body.rightArm.equippedAbility == AbilityType.LASER_GUN)
            {
                Player.Instance.body.rightArm.ActivateAbility();
            }
        }

        if (laserGunAction.WasReleasedThisFrame())
        {
            Player.Instance.abilityManager.laserGun.DisableLaserBeam();
        }
    }
}