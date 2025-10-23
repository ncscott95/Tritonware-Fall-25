using UnityEngine;

public class Box : MonoBehaviour
{
    public Attack attackComponent;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Player.Instance.body.DamagePlayerBody(attackComponent);
            Destroy(gameObject);
        }
        else if (collision.collider.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
