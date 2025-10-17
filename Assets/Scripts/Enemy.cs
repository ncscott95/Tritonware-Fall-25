using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Attack attackComponent;
    public Health healthComponent;
    public Hitbox hitboxComponent;

    void DamagePlayer()
    {
        BodyPart bodyPart = Player.Instance.body.GetRandomBodyPart();
        Health bodyPartHealth = bodyPart.healthComponent;
        bodyPartHealth.TakeDamage(attackComponent.damage);
        Debug.Log($"damaged {bodyPart.gameObject.name} for {attackComponent.damage} damage");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            DamagePlayer();
        }
    }
}