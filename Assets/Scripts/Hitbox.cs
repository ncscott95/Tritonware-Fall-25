using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Health healthComponent;

    public void TakeDamage(Attack attackComponent)
    {
        Debug.Log("Hitbox took damage: " + attackComponent.damage);
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(attackComponent.damage);
        }
    }
}