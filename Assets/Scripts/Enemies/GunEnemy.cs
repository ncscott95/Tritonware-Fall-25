using System.Collections;
using UnityEngine;

public class GunEnemy : Enemy
{
    public Rigidbody2D rb;
    public GameObject visuals;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float moveSpeed;
    public float minFireDistance;
    public float fireChargeTime;
    public float fireCooldown;
    private bool canAct = true;
    private bool isRepositioning = false;
    [SerializeField] private AudioSource blasterAudio;

    void Update()
    {
        if (!canAct || !isAggro || target == null) return;

        float distanceToPlayer = target.position.x - transform.position.x;
        if (Mathf.Abs(distanceToPlayer) < minFireDistance)
        {
            // if stuck moving away from player, fire anyway
            if (isRepositioning && Mathf.Abs(rb.linearVelocity.x) < 0.01f)
            {
                StartCoroutine(HandleBulletFiring());
                return;
            }

            // move away from player
            isRepositioning = true;
            Vector2 moveDirection = new Vector2(distanceToPlayer * -1, 0).normalized;
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, 0);
            Vector3 newScale = visuals.transform.localScale;
            float xScale = Mathf.Abs(newScale.x) * (moveDirection.x > 0 ? -1 : 1);
            visuals.transform.localScale = new Vector3(xScale, newScale.y, newScale.z);
        }
        else
        {
            // fire at player
            Vector2 lookDirection = new Vector2(distanceToPlayer, 0).normalized;
            Vector3 newScale = visuals.transform.localScale;
            float xScale = Mathf.Abs(newScale.x) * (lookDirection.x > 0 ? -1 : 1);
            visuals.transform.localScale = new Vector3(xScale, newScale.y, newScale.z);
            StartCoroutine(HandleBulletFiring());
        }
    }

    private IEnumerator HandleBulletFiring()
    {
        canAct = false;
        isRepositioning = false;
        rb.linearVelocity = Vector2.zero;
        float fireDirection = visuals.transform.localScale.x > 0 ? -1 : 1;

        yield return new WaitForSeconds(fireChargeTime);
        EnemyBullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<EnemyBullet>();
        bullet.Initialize(fireDirection);
        blasterAudio.Play();

        yield return new WaitForSeconds(fireCooldown);
        canAct = true;
    }
}
