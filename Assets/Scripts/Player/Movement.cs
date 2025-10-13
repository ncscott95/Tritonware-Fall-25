using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Rigidbody2D rb;
    private Vector2 moveVector;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpStrength;
    public bool canJump = true;
    public Direction facingDirection
    {
        get
        {
            if (rb.linearVelocityX < 0)
            {
                return Direction.LEFT;
            }
            else
            {
                return Direction.RIGHT;
            }
        }
    }
    private Direction looking;

    // Update is called once per frame
    void Update()
    {
        moveVector = Player.Instance.controls.moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocityY);

        if (Player.Instance.controls.jumpAction.triggered && canJump)
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
