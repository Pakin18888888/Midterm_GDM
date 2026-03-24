using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Auto Run")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private float hitStunDuration = 0.25f;

    private bool isStunned = false;
    private Coroutine hitRoutine;

    [Header("Magnet Lanes")]
    [SerializeField] private MagnetLane topLane;
    [SerializeField] private MagnetLane bottomLane;
    [SerializeField] private float switchDuration = 0.2f;
    [SerializeField] private float repelHoldTime = 2.5f;

    [Header("Player Polarity")]
    [SerializeField] private MagnetPolarity polarity = MagnetPolarity.Positive;
    [SerializeField] private SpriteRenderer[] polarityIndicators;
    [SerializeField] private Light2D[] polarityLights;
    [SerializeField] private Color positiveColor = Color.red;
    [SerializeField] private Color negativeColor = Color.blue;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerAttack playerAttack;

    private bool canControl = true;
    private bool isSwitching = false;
    private bool isOnTopLane = false;
    private Coroutine switchRoutine;

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

    private void Start()
    {
        ApplyPolarityVisual();

        if (bottomLane != null && bottomLane.AttachPoint != null)
        {
            transform.position = bottomLane.AttachPoint.position;
            isOnTopLane = false;
            SetPlayerUpsideDown(false);
        }
    }

    private void FixedUpdate()
    {
        if (!canControl || !gameStarted || isStunned)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(runSpeed, 0f);
    }
    public void StartRun()
    {
        gameStarted = true;
        anim.SetTrigger(StartRunHash);
        AudioManager.I?.StartRunLoop();
    }

    public void OnJump()
    {
        if (!canControl || !gameStarted) return;
        if (isSwitching) return;
        if (topLane == null || bottomLane == null) return;

        AudioManager.I?.PlayPlayerJump();

        if (switchRoutine != null)
            StopCoroutine(switchRoutine);

        switchRoutine = StartCoroutine(CoTrySwitchLane());
    }

    private IEnumerator CoTrySwitchLane()
    {
        isSwitching = true;
        anim.SetBool(IsJumpHash, true);

        MagnetLane currentLane = isOnTopLane ? topLane : bottomLane;
        MagnetLane targetLane = isOnTopLane ? bottomLane : topLane;

        if (targetLane == null || targetLane.AttachPoint == null || currentLane == null || currentLane.AttachPoint == null)
        {
            anim.SetBool(IsJumpHash, false);
            isSwitching = false;
            yield break;
        }

        Vector3 startPos = transform.position;

        // สำคัญ: เปลี่ยนแค่ Y ไม่แตะ X
        Vector3 targetPos = new Vector3(
            transform.position.x,
            targetLane.AttachPoint.position.y,
            transform.position.z
        );

        bool canAttach = targetLane.CanAttach(polarity);

        if (canAttach)
        {
            yield return StartCoroutine(CoMoveToPosition(startPos, targetPos, switchDuration));

            isOnTopLane = !isOnTopLane;
            SetPlayerUpsideDown(isOnTopLane);
        }
        else
        {
            Vector3 pushedPos = new Vector3(
                transform.position.x,
                Mathf.Lerp(startPos.y, targetPos.y, 0.65f),
                transform.position.z
            );

            yield return StartCoroutine(CoMoveToPosition(startPos, pushedPos, switchDuration * 0.8f));
            yield return new WaitForSeconds(repelHoldTime);

            Vector3 backPos = new Vector3(
                transform.position.x,
                currentLane.AttachPoint.position.y,
                transform.position.z
            );

            yield return StartCoroutine(CoMoveToPosition(transform.position, backPos, switchDuration));

            isOnTopLane = (currentLane == topLane);
            SetPlayerUpsideDown(isOnTopLane);
        }

        anim.SetBool(IsJumpHash, false);
        isSwitching = false;
        switchRoutine = null;
    }

    private IEnumerator CoMoveToPosition(Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;
        float fixedX = transform.position.x; // ล็อก x ปัจจุบันไว้
        float fixedZ = transform.position.z;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);

            float arc = Mathf.Sin(p * Mathf.PI) * 0.25f;

            float y = Mathf.Lerp(from.y, to.y, p);

            if (to.y > from.y)
                y += arc;
            else if (to.y < from.y)
                y -= arc;

            transform.position = new Vector3(fixedX, y, fixedZ);
            yield return null;
        }

        transform.position = new Vector3(fixedX, to.y, fixedZ);
    }

    public void SetPolarity(MagnetPolarity newPolarity)
    {
        polarity = newPolarity;
        ApplyPolarityVisual();
    }

    public void TogglePolarity()
    {
        polarity = polarity == MagnetPolarity.Positive
            ? MagnetPolarity.Negative
            : MagnetPolarity.Positive;

        ApplyPolarityVisual();
        StartCoroutine(FlashEffect());

        ValidateCurrentLaneAfterPolarityChange();
    }
    private IEnumerator FlashEffect()
    {
        foreach (var sr in polarityIndicators)
        {
            sr.color = Color.white;
        }

        yield return new WaitForSeconds(0.05f);

        ApplyPolarityVisual();
    }

    private void ApplyPolarityVisual()
    {
        Color c = polarity == MagnetPolarity.Positive ? positiveColor : negativeColor;

        foreach (var sr in polarityIndicators)
        {
            if (sr != null)
                sr.color = c;
        }

        foreach (var light in polarityLights)
        {
            if (light != null)
                light.color = c;
        }
    }
    private void SetPlayerUpsideDown(bool upsideDown)
    {
        Vector3 scale = transform.localScale;
        scale.y = upsideDown ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    public MagnetPolarity GetPolarity()
    {
        return polarity;
    }

    public bool IsOnTopLane()
    {
        return isOnTopLane;
    }

    public void OnAttack()
    {
        if (!canControl || !gameStarted) return;

        anim.SetTrigger(AttackHash);
        AudioManager.I?.PlayPlayerShoot();
        playerAttack?.TryAttack();
    }

    public void PlayHit()
    {
        if (hitRoutine != null)
            StopCoroutine(hitRoutine);

        hitRoutine = StartCoroutine(CoHitStun());
    }

    private IEnumerator CoHitStun()
    {
        isStunned = true;
        anim.SetBool(IsHitHash, true);
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(hitStunDuration);

        anim.SetBool(IsHitHash, false);
        isStunned = false;
        hitRoutine = null;
    }

    public void EndHit()
    {
        anim.SetBool(IsHitHash, false);
        isStunned = false;

        if (hitRoutine != null)
        {
            StopCoroutine(hitRoutine);
            hitRoutine = null;
        }
    }

    public void PlayDeath()
    {
        canControl = false;
        anim.SetBool(IsDeadHash, true);
        AudioManager.I?.StopRunLoop();
    }

    public void SetControlEnabled(bool enabled)
    {
        canControl = enabled;

        if (!enabled)
        {
            rb.linearVelocity = Vector2.zero;
            AudioManager.I?.StopRunLoop();
        }

    }

    private void ValidateCurrentLaneAfterPolarityChange()
    {
        MagnetLane currentLane = isOnTopLane ? topLane : bottomLane;
        MagnetLane otherLane = isOnTopLane ? bottomLane : topLane;

        if (currentLane == null || otherLane == null) return;

        // ถ้ายังเกาะเลนปัจจุบันได้ ก็ไม่ต้องทำอะไร
        if (currentLane.CanAttach(polarity))
            return;

        // ถ้าเกาะเลนเดิมไม่ได้ แต่เกาะอีกเลนได้ -> ย้ายทันที
        if (otherLane.CanAttach(polarity))
        {
            if (switchRoutine != null)
                StopCoroutine(switchRoutine);

            switchRoutine = StartCoroutine(CoForceSwitchToOtherLane(otherLane));
        }
    }
    private IEnumerator CoForceSwitchToOtherLane(MagnetLane targetLane)
    {
        isSwitching = true;
        anim.SetBool(IsJumpHash, true);

        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(
            transform.position.x,
            targetLane.AttachPoint.position.y,
            transform.position.z
        );

        yield return StartCoroutine(CoMoveToPosition(startPos, targetPos, switchDuration));

        isOnTopLane = (targetLane == topLane);
        SetPlayerUpsideDown(isOnTopLane);

        anim.SetBool(IsJumpHash, false);
        isSwitching = false;
        switchRoutine = null;
    }

    public void PlayIdle()
    {
        canControl = false;
        gameStarted = false;
        isStunned = false;
        isSwitching = false;

        if (hitRoutine != null)
        {
            StopCoroutine(hitRoutine);
            hitRoutine = null;
        }

        if (switchRoutine != null)
        {
            StopCoroutine(switchRoutine);
            switchRoutine = null;
        }

        rb.linearVelocity = Vector2.zero;

        anim.SetBool(IsJumpHash, false);
        anim.SetBool(IsHitHash, false);
        anim.SetBool(IsDeadHash, false);

        anim.Play("Idle");
    }

    public void PlayWinIdleOnBottomLane()
    {
        canControl = false;
        gameStarted = false;
        isStunned = false;
        isSwitching = false;

        if (hitRoutine != null)
        {
            StopCoroutine(hitRoutine);
            hitRoutine = null;
        }

        if (switchRoutine != null)
        {
            StopCoroutine(switchRoutine);
            switchRoutine = null;
        }

        rb.linearVelocity = Vector2.zero;

        anim.SetBool(IsJumpHash, false);
        anim.SetBool(IsHitHash, false);
        anim.SetBool(IsDeadHash, false);

        // ย้ายลงเลนล่าง
        if (bottomLane != null && bottomLane.AttachPoint != null)
        {
            transform.position = new Vector3(
                transform.position.x,
                bottomLane.AttachPoint.position.y,
                transform.position.z
            );
        }

        isOnTopLane = false;
        SetPlayerUpsideDown(false);

        anim.Play("Idle");
    }
}