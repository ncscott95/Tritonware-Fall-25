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
            BodyPart bodyPart = Player.Instance.body.GetRandomBodyPart();
            Health bodyPartHealth = bodyPart.healthComponent;
            bodyPartHealth.TakeDamage(attackComponent.damage);
            Debug.Log($"damaged {bodyPart.gameObject.name} for {attackComponent.damage} damage");
        }
        Destroy(gameObject);
    }
}
