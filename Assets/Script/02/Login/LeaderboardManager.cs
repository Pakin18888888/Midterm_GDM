using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;

public class LeaderboardManager : MonoBehaviour
{
    [System.Serializable]
    public class FakeScore
    {
        public string name;
        public int score;
    }
    public static LeaderboardManager Instance;

    const string leaderboardID = "TestLeaderboard";

    List<FakeScore> fakeScores =
        new List<FakeScore>()
        {
            new FakeScore(){ name="CyberFox", score=1200 },
            new FakeScore(){ name="Nova", score=1150 },
            new FakeScore(){ name="Shadow", score=1100 },
            new FakeScore(){ name="Volt", score=500 },
            new FakeScore(){ name="Echo", score=300 },
            new FakeScore(){ name="Zen", score=250 },
            new FakeScore(){ name="Pixel", score=200 },
            new FakeScore(){ name="Ghost", score=100 },
            new FakeScore(){ name="Alpha", score=10 },
            new FakeScore(){ name="YOU", score=0 },
        };

    void Awake()
    {
        Instance = this;
    }

    // 🔥 submit แบบ safe
    public async Task SubmitScoreSafe(int score)
    {
        try
        {
            // 🔥 รอ UGS
            await UGSInitializer.InitTask;

            await LeaderboardsService.Instance
                .AddPlayerScoreAsync(
                    leaderboardID,
                    score
                );

            Debug.Log("Online Score Submitted");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    // 🌍 โหลด leaderboard
    public async Task<List<ScoreboardManager.ScoreData>> GetLeaderboard()
    {
        List<ScoreboardManager.ScoreData> result =
            new List<ScoreboardManager.ScoreData>();

        try
        {
            // 🔥 รอ UGS
            await UGSInitializer.InitTask;

            var scores =
                await LeaderboardsService.Instance
                .GetScoresAsync(
                    leaderboardID,
                    new GetScoresOptions
                    {
                        Limit = 10
                    }
                );

            foreach (var s in scores.Results)
            {
                ScoreboardManager.ScoreData data =
                    new ScoreboardManager.ScoreData();

                data.score = (int)s.Score;

                data.name = s.PlayerName;

                if (s.PlayerId ==
                    AuthenticationService.Instance.PlayerId)
                {
                    data.name = PlayerNameManager.Instance.GetName();
                }

                result.Add(data);
            }

            foreach (var bot in fakeScores)
            {
                ScoreboardManager.ScoreData data =
                    new ScoreboardManager.ScoreData();

                data.name = bot.name;
                data.score = bot.score;

                result.Add(data);
            }

            result.Sort((a, b) => b.score.CompareTo(a.score));

            if (result.Count > 10)
            {
                result = result.GetRange(0, 10);
            }
        }
        
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

        return result;
    }
}