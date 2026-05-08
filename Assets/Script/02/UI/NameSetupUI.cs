using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameSetupUI : MonoBehaviour
{
    public TMP_InputField inputField;

    public void ConfirmName()
    {
        string playerName = inputField.text;

        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerNameManager.Instance
            .SaveName(playerName);

        SceneManager.LoadScene("MainMenuScene");
    }
}