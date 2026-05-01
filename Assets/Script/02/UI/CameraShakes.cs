using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShakes : MonoBehaviour
{
    public static CameraShakes Instance;

    public CinemachineCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    void Awake()
    {
        Instance = this;
        noise = vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines(); // 🔥 กันซ้อน
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        noise.AmplitudeGain = magnitude;

        yield return new WaitForSeconds(duration);

        noise.AmplitudeGain = 0;
    }
}