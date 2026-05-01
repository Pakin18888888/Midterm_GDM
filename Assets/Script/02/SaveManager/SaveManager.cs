using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveJson(string data)
    {
        PlayerPrefs.SetString("save", data);
    }

    public string LoadJson()
    {
        return PlayerPrefs.GetString("save");
    }
}
