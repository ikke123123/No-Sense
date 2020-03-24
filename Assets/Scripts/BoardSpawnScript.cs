using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawnScript : MonoBehaviour
{
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;

    public List<Piece> GenerateBoard(LocationGrid locationGrid, Team team)
    {
        List<Piece> tempList = new List<Piece>();
        if (team == Team.Black)
        {
            for (int i = 5; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (locationGrid.locations[i, j].validLocation == true) tempList.Add(GeneratePiece(locationGrid.locations[i, j], Team.Black));
                }
            }
        } else
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (locationGrid.locations[i, j].validLocation == true) tempList.Add(GeneratePiece(locationGrid.locations[i, j], Team.White));
                }
            }
        }
        return tempList;
    }

    private Piece GeneratePiece(Location location, Team type)
    {
        GameObject tempObject = Instantiate((type == Team.White) ? whitePiecePrefab : blackPiecePrefab);
        location.isOccupied = true;
        Piece tempPiece = tempObject.GetComponent<Piece>();
        tempPiece.location = location;
        location.piece = tempPiece;
        tempObject.transform.position = location.position;
        return tempPiece;
    }
}
