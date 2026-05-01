using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5f;

    private Rigidbody2D rb;
    private bool canControl;
    private bool isRunning;
    private bool isStunned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!canControl || !isRunning || isStunned)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(runSpeed, 0f);
    }

    public void SetRunning(bool running)
    {
        isRunning = running;
    }

    public void SetControlEnabled(bool enabled)
    {
        canControl = enabled;
    }

    public void Stop()
    {
        isRunning = false;
        canControl = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void Stun(float duration = 0.25f)
    {
        if (!gameObject.activeInHierarchy) return;
        StopAllCoroutines();
        StartCoroutine(CoStun(duration));
    }

    private System.Collections.IEnumerator CoStun(float duration)
    {
        isStunned = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
}