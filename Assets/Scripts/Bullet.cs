using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private float xDirection;
    [SerializeField]
    private float speed;
    public Attack attackComponent;
    private const float LIFETIME = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Direction direction = Player.Instance.movementComponent.facingDirection;
        xDirection = direction == Direction.LEFT ? -1.0f : 1.0f;
        Destroy(gameObject, LIFETIME);
    }

    void Update()
    {
        rb.linearVelocity = new(xDirection * speed, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Hitbox hitboxComponent = collision.collider.gameObject.GetComponent<Hitbox>();

        if (hitboxComponent)
        {
            hitboxComponent.TakeDamage(attackComponent);
        }
        Destroy(gameObject);
    }
}
