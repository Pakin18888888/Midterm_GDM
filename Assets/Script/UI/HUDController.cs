using UnityEngine;
using TMPro;
using System.Collections;

public class HUDController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text msgText;
    [SerializeField] private GameObject restartButton;

    [Header("Message Settings")]
    [SerializeField] private float autoHideDelay = 1.0f;

    Coroutine _msgRoutine;

    private void Start()
    {
        if (restartButton != null)
            restartButton.SetActive(false);
    }

    public void SetCoins(int current, int target)
    {
        if (coinText != null)
            coinText.text = $"x {current}";
    }

    public void ShowWin()
    {
        ShowMessage("YOU WIN!", false);

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public void ShowDead()
    {
        ShowMessage("RESPAWNING...", true);
    }

    public void ShowGameOver()
    {
        ShowMessage("GAME OVER", false);

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public void HideRestartButton()
    {
        if (restartButton != null)
            restartButton.SetActive(false);
    }

    public void ShowMessage(string message, bool autoHide)
    {
        if (msgText == null) return;

        if (_msgRoutine != null)
            StopCoroutine(_msgRoutine);

        msgText.gameObject.SetActive(true);
        msgText.text = message;

        if (autoHide)
            _msgRoutine = StartCoroutine(HideAfterDelay(autoHideDelay));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearMessage();
    }

    public void ClearMessage()
    {
        if (msgText == null) return;

        msgText.text = "";
        msgText.gameObject.SetActive(false);
    }
}