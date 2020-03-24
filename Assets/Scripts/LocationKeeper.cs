using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationKeeper : MonoBehaviour
{
    [SerializeField, Range(1, 15)] private int gridWidth = 1;
    [SerializeField, Range(1, 15)] private int gridDepth = 1;
    [SerializeField] private BoardSpawnScript boardSpawn = null;
    [HideInInspector] public LocationGrid locationGrid = new LocationGrid();
    [HideInInspector] public List<Piece> whiteObjects;
    [HideInInspector] public List<Piece> blackObjects;

    private void Awake()
    {
        locationGrid.GenerateGrid(gridWidth, gridDepth);
        if (boardSpawn != null)
        {
            whiteObjects = boardSpawn.GenerateBoard(locationGrid, Team.White);
            blackObjects = boardSpawn.GenerateBoard(locationGrid, Team.Black);
        }
        else Debug.LogWarning("Board Spawn wasn't assigned");
    }

    private void Start()
    {
        //Debug.Log(locationGrid.locations.Length);
        //for (int i = 0; i < gridWidth; i++)
        //{
        //    for (int j = 0; j < gridDepth; j++)
        //    {
        //        Debug.Log(locationGrid.locations[i, j].validLocation + " " + i.ToString() + " " + j.ToString());
        //        Debug.Log(locationGrid.locations[i, j].position);
        //    }
        //}

    }
}
