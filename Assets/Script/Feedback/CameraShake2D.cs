using UnityEngine;
using System.Collections;

public class CameraShake2D : MonoBehaviour
{
    public static CameraShake2D I;

    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);

        originalPos = transform.localPosition;
    }

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(CoShake(intensity, duration));
    }

    IEnumerator CoShake(float intensity, float duration)
    {
        Vector3 originalPos = transform.localPosition;

        float timer = 0f;

        while (timer < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shakeRoutine = null;
    }
}