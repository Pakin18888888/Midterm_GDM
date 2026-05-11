using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager Instance;

    const string NAME_KEY = "PLAYER_NAME";

    string currentName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            currentName =
                PlayerPrefs.GetString(NAME_KEY, "");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void SaveName(string playerName)
    {
        currentName = playerName;

        PlayerPrefs.SetString(
            NAME_KEY,
            playerName
        );

        PlayerPrefs.Save();

        await UpdateOnlineName(playerName);

        Debug.Log("NAME SAVED : " + playerName);
    }

    public string GetName()
    {
        return currentName;
    }

    public void DeleteName()
    {
        currentName = "";

        PlayerPrefs.DeleteKey(NAME_KEY);

        PlayerPrefs.Save();
    }

    public async Task UpdateOnlineName(string playerName)
    {
        try
        {
            await AuthenticationService.Instance
                .UpdatePlayerNameAsync(playerName);

            Debug.Log("ONLINE NAME UPDATED");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}