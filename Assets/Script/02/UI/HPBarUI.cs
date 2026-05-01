using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    public PlayerHealths player;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start()
    {
        UpdateHearts(player.hp);
        player.OnHPChanged += UpdateHearts;
    }

    void UpdateHearts(int hp)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < hp ? fullHeart : emptyHeart;
        }
    }
}