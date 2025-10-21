using UnityEngine;

public class DropBox : Ability
{
    [SerializeField]
    private GameObject boxPrefab;
    [SerializeField]
    private float attackCooldown;
    private float elapsed;
    public bool CanUse => elapsed >= attackCooldown;

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
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
        }
    }
}
