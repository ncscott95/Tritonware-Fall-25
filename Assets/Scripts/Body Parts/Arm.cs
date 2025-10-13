using UnityEngine;

public class Arm : MonoBehaviour, BodyPart
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float attackCooldown;
    private float elapsed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Player.Instance.body.SwapArm(this);
            Destroy(gameObject);
        }
    }

    public void ActivateAbility()
    {
        Debug.Log("activating arm ability");
        if (Player.Instance.controls.primaryAttackAction.triggered && elapsed >= attackCooldown)
        {
            elapsed %= attackCooldown;
            Instantiate(bulletPrefab);
            bulletPrefab.transform.position = Player.Instance.gameObject.transform.position;
        }
    }
}
