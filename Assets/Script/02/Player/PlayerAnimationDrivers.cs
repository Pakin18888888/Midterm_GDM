using UnityEngine;

public class PlayerAnimationDrivers : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayRun()
    {
        anim.SetBool("startRun", true);
    }

    public void StopRun()
    {
        anim.SetBool("startRun", false);
    }

    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }

    public void PlayHit()
    {
        anim.SetTrigger("isHit");
    }

    public void PlayDeath()
    {
        anim.SetTrigger("death");
    }
}