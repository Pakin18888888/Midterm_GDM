using UnityEngine;

public class StageChangeTrigger : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private MagnetLane topLane;
    [SerializeField] private MagnetLane bottomLane;

    [Header("Spawner")]
    [SerializeField] private EnemySpawner stage2Spawner;

    [Header("One Time")]
    [SerializeField] private bool triggerOnce = true;

    private bool activated = false;

    private void Reset()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c != null)
            c.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (triggerOnce && activated) return;

        activated = true;

        if (topLane != null) topLane.TogglePolarity();
        if (bottomLane != null) bottomLane.TogglePolarity();

        PlayerRoot playerRoot = other.GetComponent<PlayerRoot>();
        if (playerRoot != null)
        {
            playerRoot.TogglePolarity();
            playerRoot.ForceMoveToTopLane();
        }
        else
        {
            PlayerPolarity playerPolarity = other.GetComponent<PlayerPolarity>();
            if (playerPolarity != null)
                playerPolarity.Toggle();
        }

        GameFlowController.I?.ShowMessage("PHASE 2", true);

        if (stage2Spawner != null)
            stage2Spawner.StartSpawning();

        if (triggerOnce)
            gameObject.SetActive(false);
    }
}