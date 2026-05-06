using System;
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

    // 🔥 ใช้กับ Combo
    public void ShowCombo(int multiplier)
    {
        if (multiplier <= 1) return;

        SpawnPopup("x" + multiplier, player.position + Vector3.up * 1.5f);
    }

    // 🔥 ใช้กับ Score
    public void ShowScore(int score, Vector3 worldPos)
    {
        SpawnPopup("+" + score, worldPos + Vector3.up * 1f);
    }

    // 🔥 ตัวกลาง
    void SpawnPopup(string text, Vector3 worldPos)
    {
        GameObject obj = Instantiate(comboPrefab, canvas);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        obj.transform.position = screenPos;

        obj.GetComponent<ComboPopup>().SetText(text);
    }

    internal void ShowScore(int v, object position)
    {
        throw new NotImplementedException();
    }
}