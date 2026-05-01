using UnityEngine;

public class PlayerPolaritys : MonoBehaviour
{
    public int currentPolarity = 1; // 1 = +, -1 = -

    public void Toggle()
    {
        currentPolarity *= -1;
    }
}