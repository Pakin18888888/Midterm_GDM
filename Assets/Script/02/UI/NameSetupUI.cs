using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameSetupUI : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void ConfirmName()
    {
        string playerName = nameInput.text;

        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerNameManager.Instance
            .SaveName(playerName);

        SceneManager.LoadScene("MainMenuScene");
    }
}