using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Auto Run")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private float jumpAnimDuration = 0.15f;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerAttack playerAttack;

    private bool canControl = true;
    private Coroutine jumpRoutine;

    private static readonly int IsJumpHash = Animator.StringToHash("isJump");
    private static readonly int IsHitHash = Animator.StringToHash("isHit");
    private static readonly int IsDeadHash = Animator.StringToHash("isDead");
    private static readonly int AttackHash = Animator.StringToHash("attack");
    private static readonly int StartRunHash = Animator.StringToHash("startRun");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void FixedUpdate()
    {
        if (!canControl || !gameStarted)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
    }

    public void StartRun()
    {
        gameStarted = true;
        anim.SetTrigger(StartRunHash);
    }

    public void OnJump()
    {
        if (!canControl || !gameStarted) return;

        anim.SetBool(IsJumpHash, true);

        if (jumpRoutine != null)
            StopCoroutine(jumpRoutine);

        jumpRoutine = StartCoroutine(EndJumpAfterDelay(jumpAnimDuration));

        // ตรงนี้ค่อยใส่ logic เด้งขึ้น / สลับขั้ว ทีหลัง
    }

    private IEnumerator EndJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool(IsJumpHash, false);
        jumpRoutine = null;
    }

    public void OnAttack()
    {
        if (!canControl || !gameStarted) return;

        anim.SetTrigger(AttackHash);
        playerAttack?.TryAttack();
    }

    public void PlayHit()
    {
        anim.SetBool(IsHitHash, true);
    }

    public void EndHit()
    {
        anim.SetBool(IsHitHash, false);
    }

    public void PlayDeath()
    {
        canControl = false;
        anim.SetBool(IsDeadHash, true);
    }

    public void SetControlEnabled(bool enabled)
    {
        canControl = enabled;

        if (!enabled)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
}