using UnityEngine;
using TMPro;

public class ComboPopup : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float lifeTime = 1f;
    public float floatAmplitude = 30f;
    public float floatFrequency = 3f;

    private TextMeshProUGUI text;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private float time;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        rect.localScale = Vector3.zero;

        LeanTween.scale(gameObject, Vector3.one * 1.3f, 0.15f).setEaseOutBack();
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.15f);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        time += Time.deltaTime;

        // ลอยขึ้น
        rect.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime * 100f;

        // แกว่ง
        float xOffset = Mathf.Sin(time * floatFrequency) * floatAmplitude * Time.deltaTime;
        rect.anchoredPosition += new Vector2(xOffset, 0);

        // หมุนเล็กน้อย
        rect.rotation = Quaternion.Euler(0, 0, Mathf.Sin(time * 5f) * 10f);

        // fade
        canvasGroup.alpha = 1 - (time / lifeTime);
    }

    public void SetText(string value)
    {
        text.text = value;

        // 🔥 Combo
        if (value.StartsWith("x"))
        {
            if (value == "x2")
                text.color = Color.yellow;

            else if (value == "x3")
            {
                text.color = Color.red;
                text.fontSize *= 1.5f;
            }
        }
        // 💥 Score
        else if (value.StartsWith("+"))
        {
            int score = int.Parse(value.Replace("+", ""));

            if (score >= 20)
                text.color = Color.red;
            else
                text.color = Color.yellow;

            // เด้งแรงขึ้น
            rect.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one * 1.4f, 0.15f).setEaseOutBack();
        }
    }
}