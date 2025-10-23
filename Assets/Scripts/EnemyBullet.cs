using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    private float xDirection = 0f;
    public Attack attackComponent;
    private const float LIFETIME = 15f;

    void Start()
    {
        Destroy(gameObject, LIFETIME);
    }

    public void Initialize(float direction)
    {
        xDirection = direction;
    }

    void Update()
    {
        rb.linearVelocity = new(xDirection * speed, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Player.Instance.body.DamagePlayerBody(attackComponent);
        }
        Destroy(gameObject);
    }
}
