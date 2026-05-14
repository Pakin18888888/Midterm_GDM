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

        noise = vcam.GetComponentInChildren
        <CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines();

        StartCoroutine(
            ShakeRoutine(duration, magnitude)
        );
    }

    IEnumerator ShakeRoutine(
        float duration,
        float magnitude
    )
    {
        noise.AmplitudeGain = magnitude;
        noise.FrequencyGain = 2f;

        yield return new WaitForSeconds(duration);

        noise.AmplitudeGain = 0f;
    }

    public void ResetShake()
    {
        StopAllCoroutines();

        noise.AmplitudeGain = 0f;
    }
}