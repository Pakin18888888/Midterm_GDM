using UnityEngine;

public class PlayerAnimationDriver : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int StartRunHash = Animator.StringToHash("startRun");
    private static readonly int AttackHash = Animator.StringToHash("attack");
    private static readonly int HitHash = Animator.StringToHash("isHit");
    private static readonly int DeadHash = Animator.StringToHash("death");
    private static readonly int JumpHash = Animator.StringToHash("isJump");

    private void Reset()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void PlayStartRun()
    {
        if (animator == null) return;
        animator.SetTrigger(StartRunHash);
    }

    public void PlayAttack()
    {
        if (animator == null) return;
        animator.SetTrigger(AttackHash);
    }

    public void PlayHit()
    {
        if (animator == null) return;
        animator.SetBool(HitHash, true);
    }

    public void EndHit()
    {
        if (animator == null) return;
        animator.SetBool(HitHash, false);
    }

    public void PlayDeath()
    {
        if (animator == null) return;

        animator.SetBool(HitHash, false);
        animator.SetBool(JumpHash, false);
        animator.ResetTrigger(AttackHash);
        animator.ResetTrigger(StartRunHash);

        animator.SetTrigger(DeadHash);
    }

    public void SetJump(bool value)
    {
        if (animator == null) return;
        animator.SetBool(JumpHash, value);
    }

    public void PlayIdle()
    {
        if (animator == null) return;

        animator.ResetTrigger(StartRunHash);
        animator.ResetTrigger(AttackHash);

        animator.SetBool(JumpHash, false);
        animator.SetBool(HitHash, false);

        animator.CrossFade("Idle", 0.05f);
    }
}