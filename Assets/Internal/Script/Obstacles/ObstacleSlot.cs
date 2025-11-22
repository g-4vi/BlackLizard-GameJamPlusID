using UnityEngine;

public class ObstacleSlot 
{
    
    bool _occupied;
    public bool OccupiedStatus => _occupied;

    public void ChangeOccupyStatus(bool occupied)
    {
        _occupied = occupied;
    }

}
