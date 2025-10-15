using UnityEngine;

public class Blaster : Ability
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float attackCooldown;
    private float elapsed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
    }

    public override void ActivateAbility()
    {
        if (elapsed >= attackCooldown)
        {
            elapsed %= attackCooldown;
            Instantiate(bulletPrefab, Player.Instance.transform.position, Quaternion.identity);
        }
    }
}
