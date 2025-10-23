using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LaserEnemy : Enemy
{
    public Rigidbody2D rb;
    public GameObject visuals;
    public float moveSpeed;
    [SerializeField]
    private EnemyLaserBeam laserBeam;
    public float aggroDistance;
    public float fireChargeTime;
    public float fireCooldown;
    private bool canAct = true;
    private bool isRepositioning = false;

    void Update()
    {
        if (!canAct || !isAggro || target == null) return;

        float distanceToPlayer = target.position.x - transform.position.x;
        if (Mathf.Abs(distanceToPlayer) > aggroDistance)
        {
            // move towards player
            isRepositioning = true;
            Vector2 moveDirection = new Vector2(distanceToPlayer * 1, 0).normalized;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, 0);
            Vector3 newScale = visuals.transform.localScale;
            float xScale = Mathf.Abs(newScale.x) * (moveDirection.x > 0 ? -1 : 1);
            visuals.transform.localScale = new Vector3(xScale, newScale.y, newScale.z);
            laserBeam.SetLaserBeamActive(false);

        }
        else
        {
            rb.linearVelocity = new(0, 0);
            laserBeam.SetLaserBeamActive(true);

            if (laserBeam.playerCollidedWithBeam)
            {
                DamagePlayer();
            }
        }
    }

    // private IEnumerator HandleBulletFiring()
    // {
    //     canAct = false;
    //     isRepositioning = false;
    //     rb.linearVelocity = Vector2.zero;
    //     float fireDirection = visuals.transform.localScale.x > 0 ? -1 : 1;

    //     yield return new WaitForSeconds(fireChargeTime);
    //     // EnemyBullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<EnemyBullet>();
    //     // bullet.Initialize(fireDirection);

    //     yield return new WaitForSeconds(fireCooldown);
    //     canAct = true;
    // }
}
