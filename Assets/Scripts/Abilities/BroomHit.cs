using System.Collections;
using UnityEngine;

public class BroomHit : Ability
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Animator animator;
    [SerializeField] private Broom broomHitbox;
    [SerializeField] private float attackWindup;
    [SerializeField] private float attackDuration;
    private float elapsed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elapsed = attackCooldown;
        SetBroomHitboxActive(false);
    }

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
            animator.SetTrigger("broomHit");
            StartCoroutine(HandleHitboxDuration());
        }
    }

    private IEnumerator HandleHitboxDuration()
    {
        yield return new WaitForSeconds(attackWindup);
        SetBroomHitboxActive(true);
        yield return new WaitForSeconds(attackDuration);
        SetBroomHitboxActive(false);
    }

    public void SetBroomHitboxActive(bool isActive)
    {
        broomHitbox.gameObject.SetActive(isActive);
    }

    protected override void ShowAbilityUI()
    {
    }

    public override void ResetAbility()
    {
    }
}
