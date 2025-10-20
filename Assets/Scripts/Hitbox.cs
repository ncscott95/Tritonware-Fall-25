using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Health healthComponent;
    [SerializeField]
    private float immunityTime = 1f;
    private float elapsed;

    void Update()
    {
        elapsed += Time.deltaTime;
    }


    public void TakeDamage(Attack attackComponent)
    {
        if (healthComponent != null && elapsed >= immunityTime)
        {
            elapsed %= immunityTime;
            healthComponent.TakeDamage(attackComponent.damage);
        }
    }
}