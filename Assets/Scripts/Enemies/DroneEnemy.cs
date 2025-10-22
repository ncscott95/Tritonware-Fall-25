using System.Collections;
using UnityEngine;

public class DroneEnemy : Enemy
{
    public DropBox dropBoxAbility;
    public Rigidbody2D rb;
    public float moveSpeed;
    public float maxDropDistance;
    public float dropChargeTime;
    public float dropRecoveryTime;
    private bool canMove = true;

    void Update()
    {
        if (!canMove || !isAggro || target == null) return;

        float distanceToPlayer = target.position.x - transform.position.x;
        if (Mathf.Abs(distanceToPlayer) > maxDropDistance)
        {
            // move towards player
            Vector2 moveDirection = new Vector2(distanceToPlayer, 0).normalized;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, 0);
        }
        else
        {
            // drop box on player
            if (dropBoxAbility.CanUse)
            {
                StartCoroutine(HandleDropCharge());
            }
        }
    }

    private IEnumerator HandleDropCharge()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(dropChargeTime);

        // drop box ability
        dropBoxAbility.ActivateAbility();

        yield return new WaitForSeconds(dropRecoveryTime);
        canMove = true;
    }

    protected override void OnSetAggro(bool isAggro)
    {
        if (!isAggro)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
