using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public int streak = 0;

    public System.Action<int> OnStreakChanged;

    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        streak++;

        OnStreakChanged?.Invoke(streak);
        int multiplier = GetMultiplier();
        score += amount * multiplier;

        if (ComboEffect.Instance != null)
            ComboEffect.Instance.Trigger(streak);

        if (ComboManager.Instance != null)
            ComboManager.Instance.ShowCombo(multiplier);

        // 💥 จอสั่นตอน x3
        // if (multiplier == 3 && CameraShakes.Instance != null)
        // {
        //     CameraShakes.Instance.Shake(.5f, 4f);
        // }

        var effect = FindObjectOfType<PlayerEffect>();
        if (effect != null)
            effect.PlayComboEffect();
    }

    int GetMultiplier()
    {
        if (streak > 15) return 3;
        if (streak > 7) return 2;
        return 1;
    }

    public void ResetStreak()
    {
        streak = 0;

        OnStreakChanged?.Invoke(streak);
    }
}