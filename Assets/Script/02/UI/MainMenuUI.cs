using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject profilePanel;
    public GameObject leaderboardPanel;
    public RectTransform logo;

    void Start()
    {

        LeanTween.moveY(
            logo,
            logo.anchoredPosition.y + 20f,
            1f
        )
        .setLoopPingPong()
        .setEaseInOutSine();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenLeaderboard()
    {
        leaderboardPanel.SetActive(true);

        LeanTween.scale(
            leaderboardPanel,
            Vector3.one,
            0.25f
        ).setEaseOutBack();

        FindObjectOfType<LeaderboardUI>()
            .LoadOnline();
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }

    public void OpenProfile()
    {
        profilePanel.SetActive(true);
    }

    public void CloseProfile()
    {
        profilePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}