using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private float moveDownDelay = 0.25f;

    private bool activated = false;

    private void Reset()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c != null)
            c.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated && triggerOnce) return;
        if (!other.CompareTag(playerTag)) return;

        activated = true;

        PlayerRoot playerRoot = other.GetComponent<PlayerRoot>();
        if (playerRoot != null)
        {
            StartCoroutine(CoFinish(playerRoot));
        }
        else
        {
            GameFlowController.I?.Win();
        }
    }

    private IEnumerator CoFinish(PlayerRoot playerRoot)
    {
        playerRoot.ForceMoveToBottomLane();

        yield return new WaitForSeconds(moveDownDelay);

        playerRoot.OnWin();
        GameFlowController.I?.Win();
    }
}