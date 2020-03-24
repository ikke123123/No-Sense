using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [HideInInspector] public Location location;
    [HideInInspector] public List<PossibleMove> possibleMoves = new List<PossibleMove>();
    [HideInInspector] public bool isSpecialPiece = false;
    [HideInInspector] public Team team;

    public bool MoveTo(Location target)
    {
        if (target.isOccupied) return false; 
        location.LeaveLocation();
        target.OccupyLocation(this);
        transform.position = target.position;
        location = target;
        return true;
    }

    public void MakeSpecialPiece()
    {
        //Put stuff in here
    }
}
