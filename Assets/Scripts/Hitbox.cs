using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Health healthComponent;

    public void TakeDamage(Attack attackComponent)
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(attackComponent.damage);
        }
    }
}