using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class ProfileUI : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text playerNameText;
    public TMP_Text accountTypeText;
    public TMP_Text highScoreText;
    public TMP_Text bestStreakText;

    [Header("Input")]
    public TMP_InputField nameInput;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        // 👤 Player Name
        playerNameText.text =
            "Player : " +
            PlayerNameManager.Instance.GetName();

        // 🏆 High Score
        highScoreText.text =
            "High Score : " +
            ScoreboardManager.Instance.GetHighScore();

        // ⚡ Best Streak
        bestStreakText.text =
            "Best Streak : " +
            ScoreboardManager.Instance.GetBestStreak();

        // 🔐 Account Type
        if (AuthenticationService.Instance.IsSignedIn)
        {
            accountTypeText.text =
                "Account : Guest";
        }
        else
        {
            accountTypeText.text =
                "Account : Offline";
        }
    }

    // =====================
    // ✏ Change Name
    // =====================
    public void ChangeName()
    {
        string newName = nameInput.text;

        if (string.IsNullOrEmpty(newName))
            return;

        PlayerNameManager.Instance
            .SaveName(newName);

        Refresh();

        Debug.Log("NAME CHANGED");
    }

    // =====================
    // 🚪 Logout
    // =====================
    public void Logout()
    {
        // 🔥 logout จริง
        AuthenticationService.Instance
            .SignOut(true);

        // 🔥 ล้างชื่อ
        PlayerNameManager.Instance
            .DeleteName();

        // 🔥 กลับ boot
        SceneManager.LoadScene("BootScene");
    }

    // =====================
    // ❌ Close Panel
    // =====================
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}