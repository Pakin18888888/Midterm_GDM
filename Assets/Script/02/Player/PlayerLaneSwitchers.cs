using UnityEngine;

public class PlayerLaneSwitchers : MonoBehaviour
{
    public float laneOffset = 2f;
    private int currentLane = 0;

    public void TrySwitchLane()
    {
        currentLane = currentLane == 0 ? 1 : 0;

        Vector3 pos = transform.position;
        pos.y = currentLane == 0 ? 0 : laneOffset;
        transform.position = pos;
    }
}