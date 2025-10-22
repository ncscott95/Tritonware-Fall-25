using UnityEngine;

public class Broom : MonoBehaviour
{
    public Attack attackComponent;
    public float knockbackForce;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Hitbox hitboxComponent = collider.gameObject.GetComponent<Hitbox>();

        if (hitboxComponent)
        {
            hitboxComponent.TakeDamage(attackComponent);
            Rigidbody2D rb = hitboxComponent.GetComponent<Rigidbody2D>();

            // TODO: hitflash, screenshake, sound effect

            if (rb)
            {
                float knockbackDirection = Player.Instance.movementComponent.facingDirection == Direction.LEFT ? -1.0f : 1.0f;
                rb.AddForce(new Vector2(knockbackDirection * knockbackForce, knockbackForce / 2f), ForceMode2D.Impulse);
            }
        }
    }
}
