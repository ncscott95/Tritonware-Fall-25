using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Attack attackComponent;
    public Health healthComponent;
    public Hitbox hitboxComponent;
    protected Transform target;
    protected bool isAggro;

    protected void DamagePlayer()
    {
        Player.Instance.body.DamagePlayerBody(attackComponent);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            DamagePlayer();
        }
    }

    public void SetAggro(bool isAggro, Transform target = null)
    {
        this.isAggro = isAggro;
        this.target = target;
    }
}