using UnityEngine;

public class ResetAnimation : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.keepAnimatorStateOnDisable = true;
    }

    void OnEnable()
    {
        animator.SetTrigger("resetAnimations");
        animator.Play("Idle", 0, 0f);
        animator.Update(0);
    }
}
