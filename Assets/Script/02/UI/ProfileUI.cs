using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class ProfileUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text accountTypeText;
    public TMP_Text highScoreText;
    public TMP_Text bestStreakText;

    public TMP_InputField nameInput;

    async void OnEnable()
    {
        await UGSInitializer.InitTask;

        Refresh();
    }

    public void Refresh()
    {
        if (PlayerNameManager.Instance != null)
        {
            playerNameText.text =
                PlayerNameManager.Instance.GetName();
        }

        if (ScoreboardManager.Instance != null)
        {
            highScoreText.text =
                "High Score : " +
                ScoreboardManager.Instance.GetHighScore();

            bestStreakText.text =
                "Best Streak : " +
                ScoreboardManager.Instance.GetBestStreak();
        }

        if (Unity.Services.Core.UnityServices.State ==
    Unity.Services.Core.ServicesInitializationState.Initialized)
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                accountTypeText.text = "Logged In";
            }
            else
            {
                accountTypeText.text = "Guest";
            }
        }
    }
    public void ChangeName()
    {
        PlayerNameManager.Instance
            .SaveName(nameInput.text);

        Refresh();
    }

    public void Logout()
    {
        AuthenticationService.Instance
            .SignOut();

        SceneManager.LoadScene("LoginScene");
    }
}