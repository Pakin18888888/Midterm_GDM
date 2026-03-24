using UnityEngine;

public class StageChangeTrigger : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private PlayerMovement playerMovement;
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

        // 1) สลับขั้วพื้นบน/ล่าง
        if (topLane != null) topLane.TogglePolarity();
        if (bottomLane != null) bottomLane.TogglePolarity();

        // 2) สลับขั้วผู้เล่น
        if (playerMovement == null)
            playerMovement = other.GetComponent<PlayerMovement>();

        if (playerMovement != null)
            playerMovement.TogglePolarity();
            
        GameManager.I?.ShowMessage("PHASE 2", true);
        
        // 3) เปิด spawner ชุดที่ 2
        if (stage2Spawner != null)
            stage2Spawner.StartSpawning();

        // ถ้าอยากให้ trigger หายไปหลังใช้
        if (triggerOnce)
            gameObject.SetActive(false);
    }
}