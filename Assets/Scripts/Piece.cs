using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Piece : MonoBehaviour
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private Vector2Int gridLocation;

    [HideInInspector] public Location location;
    [HideInInspector] public List<PossibleMove> possibleMoves = new List<PossibleMove>();
    [HideInInspector] public bool isSpecialPiece = false;
    [HideInInspector] public Team team;

    private void Update()
    {
        gridLocation = location.gridLocation;
    }

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
        isSpecialPiece = true;
        GameObject specialPart = Instantiate(prefab, transform);
        specialPart.transform.localPosition = new Vector3(0,0.3f,0);
        Destroy(specialPart.GetComponent<Piece>());
    }
}
