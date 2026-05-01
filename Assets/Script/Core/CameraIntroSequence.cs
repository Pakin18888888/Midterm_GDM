using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraIntroSequence : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera finishCam;

    [Header("Timing")]
    [SerializeField] private float playerViewTime = 1.5f;
    [SerializeField] private float finishViewTime = 2f;
    [SerializeField] private float returnDelay = 0.5f;

    public IEnumerator PlayIntro()
    {
        SetCamera(playerCam);
        yield return new WaitForSeconds(playerViewTime);

        SetCamera(finishCam);
        yield return new WaitForSeconds(finishViewTime);

        SetCamera(playerCam);
        yield return new WaitForSeconds(returnDelay);
    }

    private void SetCamera(CinemachineCamera target)
    {
        if (playerCam != null)
            playerCam.Priority = (target == playerCam) ? 20 : 10;

        if (finishCam != null)
            finishCam.Priority = (target == finishCam) ? 20 : 10;
    }
}