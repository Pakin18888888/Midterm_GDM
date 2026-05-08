using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject profilePanel;
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