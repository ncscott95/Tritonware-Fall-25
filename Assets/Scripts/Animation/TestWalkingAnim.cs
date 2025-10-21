using System.Collections.Generic;
using UnityEngine;

public class TestWalkingAnim : MonoBehaviour
{
    private Animator animator;
    private bool isWalking = false;
    public Transform legsObject;
    private bool usingEnemyLegs = false;

    // 0 = back upper, 1 = back lower, 2 = front upper, 3 = front lower
    public List<SpriteRenderer> legsSpriteRenderers;
    public List<Sprite> baseLegsSprites;
    public List<Sprite> enemyLegsSprites;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle", 0);
        animator.Play("Idle", 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) isWalking = !isWalking;
        animator.SetBool("isWalking", isWalking);

        if (Input.GetKeyDown(KeyCode.L)) SwitchLegs();
    }

    private void SwitchLegs()
    {
        usingEnemyLegs = !usingEnemyLegs;

        List<Sprite> selectedSprites = usingEnemyLegs ? enemyLegsSprites : baseLegsSprites;
        for (int i = 0; i < legsSpriteRenderers.Count; i++)
        {
            legsSpriteRenderers[i].sprite = selectedSprites[i];
        }

        legsObject.localScale = new Vector3(usingEnemyLegs ? -1 : 1, 1, 1);
        animator.SetBool("usingEnemyLegs", usingEnemyLegs);
        animator.SetTrigger("resetCycle");
    }
}
