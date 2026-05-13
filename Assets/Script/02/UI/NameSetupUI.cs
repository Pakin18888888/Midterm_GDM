using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameSetupUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_Text errorText;
    [SerializeField] private RectTransform confirmButton;

    private Vector3 confirmOriginalScale;

    const int MIN_NAME = 3;
    const int MAX_NAME = 12;

    void Start()
    {
        nameInput.characterLimit = MAX_NAME;

        errorText.text = "";

        // 🔥 โหลดชื่อเดิม
        nameInput.text =
            PlayerNameManager.Instance.GetName();

        confirmOriginalScale =
            confirmButton.localScale;

        JellyLoop();
    }

    void JellyLoop()
    {
        if (confirmButton == null)
            return;

        Vector3 squash =
            new Vector3(
                confirmOriginalScale.x * 0.92f,
                confirmOriginalScale.y * 1.08f,
                1f
            );

        Vector3 stretch =
            new Vector3(
                confirmOriginalScale.x * 1.08f,
                confirmOriginalScale.y * 0.92f,
                1f
            );

        LeanTween.scale(
            confirmButton,
            squash,
            0.15f
        ).setEaseInOutSine()
        .setOnComplete(() =>
        {
            if (confirmButton == null)
                return;

            LeanTween.scale(
                confirmButton,
                stretch,
                0.15f
            ).setEaseInOutSine()
            .setOnComplete(() =>
            {
                if (confirmButton == null)
                    return;

                LeanTween.scale(
                    confirmButton,
                    confirmOriginalScale,
                    0.15f
                ).setEaseOutBack();
            });
        });

        LeanTween.delayedCall(5f, () =>
        {
            if (confirmButton == null)
                return;

            JellyLoop();
        });
    }

    public async void ConfirmName()
    {
        string playerName =
            nameInput.text.Trim();

        // 🔥 empty
        if (string.IsNullOrEmpty(playerName))
        {
            ShowError("Enter your name");
            return;
        }

        // 🔥 short
        if (playerName.Length < MIN_NAME)
        {
            ShowError("Minimum 3 characters");
            return;
        }

        // 🔥 validate
        if (!IsValidName(playerName))
        {
            ShowError(
                "Only Thai / English / Number"
            );

            return;
        }

        // 🔥 save
        await PlayerNameManager.Instance
            .SaveName(playerName);

        SceneManager.LoadScene(
            "MainMenuScene"
        );
    }

    bool IsValidName(string name)
    {
        foreach (char c in name)
        {
            bool isEnglish =
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z');

            bool isNumber =
                (c >= '0' && c <= '9');

            bool isThai =
                (c >= '\u0E00' && c <= '\u0E7F');

            bool isAllowed =
                c == '_' || c == ' ';

            if (!(isEnglish ||
                    isNumber ||
                    isThai ||
                    isAllowed))
            {
                return false;
            }
        }

        return true;
    }

    void ShowError(string msg)
    {
        errorText.text = msg;

        Debug.Log(msg);
    }
}