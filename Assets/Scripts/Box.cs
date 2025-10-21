using UnityEngine;

public class Box : MonoBehaviour
{
    public Attack attackComponent;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            BodyPart bodyPart = Player.Instance.body.GetRandomBodyPart();
            Health bodyPartHealth = bodyPart.healthComponent;
            bodyPartHealth.TakeDamage(attackComponent.damage);
            Debug.Log($"damaged {bodyPart.gameObject.name} for {attackComponent.damage} damage");
            Destroy(gameObject);
        }
        else if (collision.collider.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
