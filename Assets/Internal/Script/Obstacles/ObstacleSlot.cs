using UnityEngine;

public class ObstacleSlot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool _occupied;
    [SerializeField] GameObject _indicatorSlot;
    public void ChangeOccupyStatus(bool occupied)
    {
        _occupied = occupied;
    }
}
