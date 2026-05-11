using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager Instance;

    private const string HIGH_SCORE_KEY = "HIGH_SCORE";
    private const string BEST_STREAK_KEY = "BEST_STREAK";
    List<string> namePool = new List<string> { "Alex", "Kai", "Nova", "Zen", "Max", "Luna", "Rex" };

    string GetUniqueName()
    {
        if (namePool.Count == 0)
            return "Bot";

        int index = Random.Range(0, namePool.Count);
        string name = namePool[index];
        namePool.RemoveAt(index); // ❗ เอาออก = ไม่ซ้ำ

        return name;
    }

    private SaveManager saveManager;

    [System.Serializable]
    public class ScoreData
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class ScoreList
    {
        public List<ScoreData> scores = new List<ScoreData>();
    }

    public ScoreList leaderboard = new ScoreList();

    void Awake()
    {
        // PlayerPrefs.DeleteAll();
        // saveManager.DeleteSave();

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
        
        // 🔥 หลังจาก singleton พร้อมแล้ว
        saveManager = FindObjectOfType<SaveManager>();

        if (leaderboard == null)
        {
            leaderboard = new ScoreList();
        }

        

        GenerateFakeScores();
    }

    // =========================
    // 🏆 SAVE ALL (เรียกครั้งเดียวจบ)
    // =========================
    public void Save(int score, int streak)
    {
        SaveHighScore(score);
        SaveBestStreak(streak);
        SaveLeaderboard(score);

        PlayerPrefs.Save();
    }

    // =========================
    // HIGH SCORE
    // =========================
    void SaveHighScore(int score)
    {
        int current = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        if (score > current)
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    // =========================
    // BEST STREAK
    // =========================
    void SaveBestStreak(int streak)
    {
        int current = PlayerPrefs.GetInt(BEST_STREAK_KEY, 0);

        if (streak > current)
            PlayerPrefs.SetInt(BEST_STREAK_KEY, streak);
    }

    public int GetBestStreak()
    {
        return PlayerPrefs.GetInt(BEST_STREAK_KEY, 0);
    }

    // =========================
    // LEADERBOARD (TOP 5)
    // =========================
    void SaveLeaderboard(int score)
    {
        LoadLeaderboard();

        // 🔥 เอาค่าที่ดีที่สุด
        int bestScore = Mathf.Max(score, GetHighScore());

        // 🔍 หา YOU เดิม
        ScoreData existing = leaderboard.scores.Find(
            s => s.name ==
            PlayerNameManager.Instance.GetName()
        );

        if (existing != null)
        {
            if (bestScore > existing.score)
            {
                existing.score = bestScore;
            }
        }
        else
        {
            ScoreData newScore = new ScoreData();
            newScore.name = PlayerNameManager.Instance.GetName();
            newScore.score = bestScore;

            leaderboard.scores.Add(newScore);
        }

        leaderboard.scores.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboard.scores.Count > 20)
            leaderboard.scores.RemoveRange(20, leaderboard.scores.Count - 20);

        string json = JsonUtility.ToJson(leaderboard);
        saveManager.SaveJson(json);
    }

    public void LoadLeaderboard()
    {
        string json = saveManager.LoadJson();

        if (string.IsNullOrEmpty(json))
        {
            leaderboard = new ScoreList();
        }
        else
        {
            leaderboard = JsonUtility.FromJson<ScoreList>(json);
        }
    }

    void GenerateFakeScores()
    {
        LoadLeaderboard();

        // ถ้ามีแต่น้อยเกินไป → เติม bot
        if (leaderboard.scores.Count < 5)
        {
            for (int i = leaderboard.scores.Count; i < 10; i++)
            {
                ScoreData bot = new ScoreData();
                bot.name = GetUniqueName();
                bot.score = Random.Range(200, 2000);

                leaderboard.scores.Add(bot);
            }

            leaderboard.scores.Sort((a, b) => b.score.CompareTo(a.score));

            string json = JsonUtility.ToJson(leaderboard);
            saveManager.SaveJson(json);
        }
    }

    public void SaveOnlineLeaderboard(
    List<ScoreData> onlineScores)
    {
        if (onlineScores == null)
        {
            Debug.LogError(
                "ONLINE SCORES NULL"
            );

            return;
        }

        if (leaderboard == null)
        {
            leaderboard = new ScoreList();
        }

        leaderboard.scores = onlineScores;

        string json =
            JsonUtility.ToJson(leaderboard);

        if (saveManager != null)
        {
            saveManager.SaveJson(json);
        }
        else
        {
            Debug.LogError(
                "SAVE MANAGER NULL"
            );
        }

        Debug.Log("Online Leaderboard Saved");
    }

    public void ResetAllData()
    {
        // 🔥 ลบ PlayerPrefs
        PlayerPrefs.DeleteAll();

        // 🔥 ลบ save json
        if (saveManager != null)
        {
            saveManager.DeleteSave();
        }

        // 🔥 reset leaderboard
        leaderboard = new ScoreList();

        Debug.Log("ALL DATA RESET");
    }
}