using UnityEngine;

public class TestWalkingAnim : MonoBehaviour
{
    private Animator animator;
    private bool isWalking = false;

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
    }
}
