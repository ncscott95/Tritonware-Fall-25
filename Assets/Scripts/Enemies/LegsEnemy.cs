using System.Collections;
using UnityEngine;

public class LegsEnemy : Enemy
{
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed;
    public float maxJumpDistance;
    public float jumpHeight;
    public float jumpChargeTime;
    public float jumpRecoveryTime;
    private bool canMove = true;

    void Update()
    {
        if (!canMove || !isAggro || target == null) return;

        Vector2 distanceToPlayer = target.position - transform.position;
        if (distanceToPlayer.magnitude > maxJumpDistance)
        {
            // move towards player
            Vector2 moveDirection = distanceToPlayer.normalized;
            animator.gameObject.transform.localScale = new Vector3(moveDirection.x > 0 ? -1 : 1, 1, 1);
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocityY);
            if (rb.linearVelocity.x != 0) animator.SetBool("isWalking", true);
        }
        else
        {
            // jump towards player
            animator.SetBool("isWalking", false);
            StartCoroutine(HandleJumpCharge());
        }
    }

    private IEnumerator HandleJumpCharge()
    {
        canMove = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        Vector2 targetPosition = target.position;
        animator.SetBool("isChargingJump", true);

        yield return new WaitForSeconds(jumpChargeTime);

        // jump such that we reach jumpHeight at mid-point and land at player's position
        Vector2 jumpDirection = (targetPosition - (Vector2)transform.position).normalized;
        float horizontalDistance = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(targetPosition.x, 0));
        float timeToApex = Mathf.Sqrt((2 * jumpHeight) / Mathf.Abs(Physics2D.gravity.y));
        float totalJumpTime = timeToApex * 2;
        float horizontalVelocity = horizontalDistance / totalJumpTime;
        float verticalVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
        rb.linearVelocity = new Vector2(jumpDirection.x * horizontalVelocity, verticalVelocity);

        animator.SetBool("isChargingJump", false);
        animator.SetTrigger("jump");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            animator.SetTrigger("land");
            StartCoroutine(HandleJumpRecovery());
        }
    }

    private IEnumerator HandleJumpRecovery()
    {
        yield return new WaitForSeconds(jumpRecoveryTime);
        canMove = true;
        animator.SetTrigger("recover");
    }

    protected override void OnSetAggro(bool isAggro)
    {
        if (!isAggro)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }
}
