using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager Instance;

    const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Awake()
    {
        Instance = this;
    }

    public void SaveName(string newName)
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, newName);
        PlayerPrefs.Save();
    }

    public string GetName()
    {
        return PlayerPrefs.GetString(
            PLAYER_NAME_KEY,
            "PLAYER"
        );
    }
}