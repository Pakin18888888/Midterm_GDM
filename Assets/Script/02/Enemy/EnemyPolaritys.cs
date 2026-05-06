using UnityEngine;

public class EnemyPolaritys : MonoBehaviour
{
    public PolarityType polarity;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // 🔥 สุ่ม polarity
        polarity = Random.value > 0.5f 
            ? PolarityType.Positive 
            : PolarityType.Negative;

        ApplyVisual();
    }

    void ApplyVisual()
    {
        if (polarity == PolarityType.Positive)
            sr.color = Color.red;
        else
            sr.color = Color.blue;
    }
}