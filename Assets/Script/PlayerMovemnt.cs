using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Auto Run")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private bool gameStarted = false;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerInput playerInput;
    private PlayerAttack playerAttack;

    private InputAction jumpAction;
    private InputAction attackAction;

    private bool canControl = true;

    private static readonly int IsJumpHash = Animator.StringToHash("isJump");
    private static readonly int IsHitHash = Animator.StringToHash("isHit");
    private static readonly int IsDeadHash = Animator.StringToHash("isDead");
    private static readonly int AttackHash = Animator.StringToHash("attack");
    private static readonly int StartRunHash = Animator.StringToHash("startRun");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerAttack = GetComponent<PlayerAttack>();

        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    private void OnEnable()
    {
        if (jumpAction != null)
            jumpAction.performed += OnJump;

        if (attackAction != null)
            attackAction.performed += OnAttack;
    }

    private void OnDisable()
    {
        if (jumpAction != null)
            jumpAction.performed -= OnJump;

        if (attackAction != null)
            attackAction.performed -= OnAttack;
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

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!canControl || !gameStarted) return;

        anim.SetBool(IsJumpHash, true);

        // ตรงนี้ค่อยใส่ logic กระโดดจริง หรือสลับแรงแม่เหล็กเพิ่มทีหลัง
    }

    public void EndJump()
    {
        anim.SetBool(IsJumpHash, false);
    }

    private void OnAttack(InputAction.CallbackContext ctx)
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