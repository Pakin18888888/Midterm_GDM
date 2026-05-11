using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    string savePath =>
    Path.Combine(
        Application.persistentDataPath,
        "save.json"
    );

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        Debug.Log(savePath);
    }

    public void SaveJson(string data)
    {
        File.WriteAllText(savePath, data);
    }

    public string LoadJson()
    {
        if (File.Exists(savePath))
            return File.ReadAllText(savePath);

        return "";
    }


    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);

            Debug.Log("SAVE FILE DELETED");
        }
        else
        {
            Debug.Log("NO SAVE FILE");
        }
    }
}