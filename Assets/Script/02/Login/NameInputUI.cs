using TMPro;
using UnityEngine;

public class NameInputUI : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.text =
            PlayerNameManager.Instance.GetName();
    }

    public void Save()
    {
        PlayerNameManager.Instance
            .SaveName(inputField.text);
    }
}