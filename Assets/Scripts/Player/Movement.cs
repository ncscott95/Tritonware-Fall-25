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

    public void Move(Vector2 directionVector)
    {
        rb.linearVelocity = new(directionVector.x * moveSpeed, rb.linearVelocityY);
    }

    public void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rb.AddForceY(1 * jumpStrength, ForceMode2D.Impulse);
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
