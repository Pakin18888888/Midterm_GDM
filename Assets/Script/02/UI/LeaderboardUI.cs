using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject rowPrefab;
    public ScrollRect scrollRect;
    public Transform contentParent;
    bool hasAnimated = false;

    public async void LoadOnline()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        List<ScoreboardManager.ScoreData> scores =
            await LeaderboardManager.Instance.GetLeaderboard();

        ScoreboardManager.Instance.SaveOnlineLeaderboard(scores);

        Debug.Log("Scores Count: " + scores.Count);

        StartCoroutine(
            SpawnRows(scores, true)
        );
    }

    public void UpdateUI(bool playAnimation = false)
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        ScoreboardManager.Instance.LoadLeaderboard();
        var scores = ScoreboardManager.Instance.leaderboard.scores;

        StartCoroutine(SpawnRows(scores, playAnimation));
    }

    IEnumerator SpawnRows(List<ScoreboardManager.ScoreData> scores, bool playAnimation)
    {
        int yourIndex = -1;

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject obj = Instantiate(rowPrefab, contentParent);
            var row = obj.GetComponent<LeaderboardRowUI>();

            row.SetData(i + 1, scores[i].name, scores[i].score);

            if (scores[i].name == PlayerNameManager.Instance.GetName())
            {
                yourIndex = i;

                // 👇 เพิ่มอันนี้
                row.PlayFocusLoop();
            }

            // 🎬 เด้งทีละแถว
            if (playAnimation)
            {
                yield return new WaitForSecondsRealtime(0.05f); // ไม่โดน timeScale
                row.PlayAnimation();
            }
        }

        yield return null;
        Canvas.ForceUpdateCanvases();

        // 🎯 Scroll ไปหา YOU
        if (yourIndex != -1 && scores.Count > 1)
        {
            float normalized = 1f - (float)yourIndex / (scores.Count - 1);
            StartCoroutine(SmoothScroll(normalized));
        }

        // 🟡 Focus YOU
        if (yourIndex != -1)
        {
            FocusOnYou(yourIndex);
        }
    }

    void FocusOnYou(int index)
    {
        Transform row = contentParent.GetChild(index);

        // เด้งนิดนึง
        LeanTween.scale(row.gameObject, Vector3.one * 1.1f, 0.2f)
            .setEaseOutBack()
            .setLoopPingPong(1);

        // glow สี
        var img = row.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            Color original = img.color;
            img.color = Color.yellow;

            LeanTween.value(row.gameObject, 0f, 1f, 0.5f).setOnUpdate((float t) =>
            {
                img.color = Color.Lerp(Color.yellow, original, t);
            });
        }
    }
    IEnumerator SmoothScroll(float target)
    {
        float start = scrollRect.verticalNormalizedPosition;
        float time = 0f;

        while (time < 0.4f)
        {
            time += Time.unscaledDeltaTime;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, target, time / 0.4f);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = target;
    }
}