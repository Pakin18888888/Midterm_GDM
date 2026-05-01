using UnityEngine;
using TMPro;

public class ComboPopup : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float lifeTime = 1f;
    public float floatAmplitude = 30f; // แกว่งซ้ายขวา
    public float floatFrequency = 3f;

    private TextMeshProUGUI text;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private float time;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();

        // เพิ่ม CanvasGroup (ใช้ fade)
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        // 🔥 เด้งตอนเกิด
        rect.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one * 1.3f, 0.15f).setEaseOutBack();

        // แล้วหดกลับ
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.15f);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        time += Time.deltaTime;

        // 🎈 ลอยขึ้น
        rect.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime * 100f;

        // 🌊 แกว่งซ้ายขวา
        float xOffset = Mathf.Sin(time * floatFrequency) * floatAmplitude * Time.deltaTime;
        rect.anchoredPosition += new Vector2(xOffset, 0);

        // 🌫 fade ออก
        canvasGroup.alpha = 1 - (time / lifeTime);
    }

    public void SetText(string value)
    {
        text.text = value;

        if (value == "x2")
        {
            text.color = Color.yellow;
        }
        else if (value == "x3")
        {
            text.color = Color.red;

            // 💥 ขยายตัวอักษร
            text.fontSize *= 1.5f;
        }
    }
}