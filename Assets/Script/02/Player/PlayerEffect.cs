using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void PlayComboEffect()
    {
        sr.color = Color.yellow;
        Invoke(nameof(ResetColor), 0.1f);
    }

    void ResetColor()
    {
        sr.color = Color.white;
    }
}