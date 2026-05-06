using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
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
            File.Delete(savePath);
    }
}