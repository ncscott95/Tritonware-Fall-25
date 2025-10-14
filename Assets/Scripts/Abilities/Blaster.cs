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
        if (Player.Instance.controls.primaryAttackAction.triggered && elapsed >= attackCooldown)
        {
            Debug.Log("blaster activated");
            elapsed %= attackCooldown;
            Instantiate(bulletPrefab);
            bulletPrefab.transform.position = Player.Instance.gameObject.transform.position;
        }
    }
}
