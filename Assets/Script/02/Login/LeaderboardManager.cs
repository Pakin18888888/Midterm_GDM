using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    const string leaderboardID = "TestLeaderboard";

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
                .GetScoresAsync(leaderboardID);

            foreach (var s in scores.Results)
            {
                ScoreboardManager.ScoreData data =
                    new ScoreboardManager.ScoreData();

                data.score = (int)s.Score;

                data.name = s.PlayerName;

                if (s.PlayerId ==
                    AuthenticationService.Instance.PlayerId)
                {
                    data.name = "YOU";
                }

                result.Add(data);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

        return result;
    }
}