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

    private Vector3 targetPosition;
    private float speed = 0;
    private float step = 0;
    private float speedTarget = 0;

    private void Start()
    {
        targetPosition = location.position;
    }

    private void FixedUpdate()
    {
        gridLocation = location.gridLocation;
        speed += speed >= speedTarget ? -step : step;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
    }

    public bool MoveTo(Location target)
    {
        if (target.isOccupied) return false; 
        location.LeaveLocation();
        target.OccupyLocation(this);
        targetPosition = target.position;
        speed = 0;
        step = Vector3.Distance(transform.position, targetPosition) / 45;
        speedTarget = step * 2;
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
