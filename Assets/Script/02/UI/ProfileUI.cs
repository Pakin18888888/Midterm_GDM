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

    [Header("Confirm")]
    public GameObject confirmPanel;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        // 👤 Player Name
        string playerName =
    PlayerNameManager.Instance.GetName();

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "No Name";
        }

        playerNameText.text =
            "Player : " + playerName;

        // 🏆 High Score
        highScoreText.text =
            "High Score : " +
            ScoreboardManager.Instance.GetHighScore();

        // ⚡ Best Streak
        bestStreakText.text =
            "Best Streak : " +
            ScoreboardManager.Instance.GetBestStreak();

        // 🔐 Account Type
        accountTypeText.text = "Account : Guest";
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

        Debug.Log(PlayerNameManager.Instance.GetName());

        Refresh();

        Debug.Log("NAME CHANGED");
    }

    // =====================
    // 🚪 Logout
    // =====================
    public void Logout()
    {
        confirmPanel.SetActive(true);
    }

    public async void ConfirmDelete()
    {
        // 🔥 logout + ลบ guest session
        AuthenticationService.Instance
            .SignOut(true);

        // 🔥 ลบข้อมูล local
        PlayerNameManager.Instance
            .DeleteName();

        ScoreboardManager.Instance
            .ResetAllData();

        // 🔥 สร้าง guest account ใหม่
        await AuthenticationService.Instance
            .SignInAnonymouslyAsync();

        Debug.Log("NEW GUEST ACCOUNT CREATED");

        // 🔥 ไปหน้าเริ่ม
        SceneManager.LoadScene("START");
    }

    public void CancelDelete()
    {
        confirmPanel.SetActive(false);
    }

    // =====================
    // ❌ Close Panel
    // =====================
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }


}