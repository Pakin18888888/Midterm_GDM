using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance;

    public GameObject comboPrefab;
    public Transform canvas;
    public Transform player;



    void Awake()
    {
        Instance = this;
    }

    public void ShowCombo(int multiplier)
    {
        if (multiplier <= 1) return;

        GameObject obj = Instantiate(comboPrefab, canvas);

        // 🔥 แปลงตำแหน่ง world → screen
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position + Vector3.up * 1.5f);

        obj.transform.position = screenPos;

        obj.GetComponent<ComboPopup>().SetText("x" + multiplier);
    }
}