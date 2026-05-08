using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject profilePanel;

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