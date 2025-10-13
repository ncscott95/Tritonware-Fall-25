using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector2 moveVector;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpStrength;
    private bool canJump = true;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocityY);

        if (jumpAction.triggered && canJump)
        {
            canJump = false;
            rb.linearVelocityY = 1 * jumpStrength;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }

}
