using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] private RectTransform logo;
    [SerializeField] private RectTransform startButton;

    private Vector3 startOriginalScale;

    private void Start()
    {
        LeanTween.moveY(
            logo,
            logo.anchoredPosition.y + 20f,
            1f
        )
        .setLoopPingPong()
        .setEaseInOutSine();

        startOriginalScale =
            startButton.localScale;

        JellyLoop();
    }

    void JellyLoop()
    {
        if (startButton == null)
            return;

        Vector3 squash =
            new Vector3(
                startOriginalScale.x * 0.92f,
                startOriginalScale.y * 1.08f,
                1f
            );

        Vector3 stretch =
            new Vector3(
                startOriginalScale.x * 1.08f,
                startOriginalScale.y * 0.92f,
                1f
            );

        LeanTween.scale(
            startButton,
            squash,
            0.15f
        ).setEaseInOutSine()
        .setOnComplete(() =>
        {
            if (startButton == null)
                return;

            LeanTween.scale(
                startButton,
                stretch,
                0.15f
            ).setEaseInOutSine()
            .setOnComplete(() =>
            {
                if (startButton == null)
                    return;

                LeanTween.scale(
                    startButton,
                    startOriginalScale,
                    0.15f
                ).setEaseOutBack();
            });
        });

        LeanTween.delayedCall(5f, () =>
        {
            if (startButton == null)
                return;

            JellyLoop();
        });
    }

    void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }

    public async void StartGame()
    {
        await UGSInitializer.InitTask;

        if (string.IsNullOrEmpty(
            PlayerNameManager.Instance.GetName()))
        {
            SceneManager.LoadScene("NameScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}