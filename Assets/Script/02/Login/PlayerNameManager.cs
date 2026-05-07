using UnityEngine;
using Unity.Services.Authentication;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager Instance;

    const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Awake()
    {
        Instance = this;
    }

    public async void SaveName(string newName)
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, newName);

        await UGSInitializer.InitTask;

        await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);
        
        PlayerPrefs.Save();
        Debug.Log("Name Updated");
    }

    public string GetName()
    {
        return PlayerPrefs.GetString(
            PLAYER_NAME_KEY,
            "PLAYER"
        );
    }
}