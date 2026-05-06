using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PolarityButtonUI : MonoBehaviour
{
    public Image buttonImage;
    public Image glowImage;

    public Color positiveColor = Color.red;
    public Color negativeColor = Color.blue;

    public float glowDuration = 0.2f;
    

    void OnEnable()
    {
        StartCoroutine(Init());
    }

    void OnDisable()
    {
        if (PlayerPolaritys.Instance != null)
            PlayerPolaritys.Instance.OnPolarityChanged -= UpdateUI;
    }

    IEnumerator Init()
    {
        // 🔥 รอจน Player พร้อม
        while (PlayerPolaritys.Instance == null)
            yield return null;

        PlayerPolaritys.Instance.OnPolarityChanged += UpdateUI;

        UpdateUI(PlayerPolaritys.Instance.currentPolarity);
    }
    void UpdateUI(PolarityType polarity)
    {
        if (polarity == PolarityType.Positive)
            buttonImage.color = positiveColor;
        else
            buttonImage.color = negativeColor;

        StopAllCoroutines();
        StartCoroutine(GlowEffect());
    }

    IEnumerator GlowEffect()
    {
        if (glowImage == null) yield break;

        glowImage.gameObject.SetActive(true);

        float t = 0;
        while (t < glowDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / glowDuration);

            Color c = glowImage.color;
            c.a = alpha;
            glowImage.color = c;

            yield return null;
        }

        glowImage.gameObject.SetActive(false);
    }
}