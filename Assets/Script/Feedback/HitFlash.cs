using UnityEngine;
using System.Collections;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    private Color originalColor;
    private Coroutine flashRoutine;

    private void Awake()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            originalColor = sr.color;
    }

    public void Play()
    {
        if (sr == null) return;

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
        flashRoutine = null;
    }
}