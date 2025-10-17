using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Rigidbody2D rb;
    private Vector2 moveVector;
    public float moveSpeed;
    public float moveSpeedMultiplier = 1f;
    [SerializeField]
    private float jumpStrength;
    public float jumpStrengthMultiplier = 1;
    public bool canJump = true;
    public Direction facingDirection;


    private Direction looking;

    public void Move(Vector2 directionVector)
    {
        rb.linearVelocity = new(directionVector.x * moveSpeed * moveSpeedMultiplier, rb.linearVelocityY);
        SetFacingDirection(directionVector);
    }

    private void SetFacingDirection(Vector2 directionVector)
    {
        if (directionVector == Vector2.zero) return;

        facingDirection = directionVector.x < 0 ? Direction.LEFT : Direction.RIGHT;
    }

    public void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rb.AddForceY(1 * jumpStrength * jumpStrengthMultiplier, ForceMode2D.Impulse);
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
