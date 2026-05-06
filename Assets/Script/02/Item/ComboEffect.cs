using UnityEngine;

public class ComboEffect : MonoBehaviour
{
    public static ComboEffect Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Trigger(int streak)
    {
        if (streak >= 10)
        {
            CameraShakes.Instance.Shake(0.15f, 0.15f);
        }

        if (streak >= 20)
        {
            CameraShakes.Instance.Shake(0.25f, 0.25f);
        }
    }
}