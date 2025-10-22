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

        Direction oldFacingDirection = facingDirection;
        facingDirection = directionVector.x < 0 ? Direction.LEFT : Direction.RIGHT;
        if (oldFacingDirection == facingDirection) return;

        Vector3 newScale = Player.Instance.visuals.transform.localScale;
        float xScale = Mathf.Abs(newScale.x) * (facingDirection == Direction.LEFT ? -1 : 1);
        Player.Instance.visuals.transform.localScale = new Vector3(xScale, newScale.y, newScale.z);
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
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }
    }

}
