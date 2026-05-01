using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text countdownText;

    [Header("Timing")]
    [SerializeField] private float stepDuration = 1f;
    [SerializeField] private string[] steps = { "3", "2", "1", "GO" };

    public void Begin(Action onFinished)
    {
        StartCoroutine(CoBegin(onFinished));
    }

    private IEnumerator CoBegin(Action onFinished)
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        for (int i = 0; i < steps.Length; i++)
        {
            if (countdownText != null)
                countdownText.text = steps[i];

            yield return new WaitForSeconds(stepDuration);
        }

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        onFinished?.Invoke();
    }
}