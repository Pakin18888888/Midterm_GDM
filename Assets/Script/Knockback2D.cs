using UnityEngine;
using System.Collections;

public class Knockback2D : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float force = 10f;
    [SerializeField] private float lockTime = 0.1f;

    private Coroutine knockRoutine;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    public void Apply(Vector2 direction)
    {
        if (knockRoutine != null)
            StopCoroutine(knockRoutine);

        knockRoutine = StartCoroutine(KnockRoutine(direction));
    }

    private IEnumerator KnockRoutine(Vector2 direction)
    {
        if (direction == Vector2.zero)
            direction = Vector2.right; // กันบัค

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(lockTime);

        knockRoutine = null;
    }
}