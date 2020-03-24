using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [HideInInspector] public Location location;

    public bool MoveTo(Location target)
    {
        if (target.isOccupied) return false; 
        location.LeaveLocation();
        target.OccupyLocation(this);
        transform.position = target.position;
        return true;
    }
}
