using UnityEngine;

public class EnemyLaneInfo : MonoBehaviour
{
    [SerializeField] private bool isTopLane;

    public bool IsTopLane => isTopLane;

    public void SetLane(bool top)
    {
        isTopLane = top;
    }
}