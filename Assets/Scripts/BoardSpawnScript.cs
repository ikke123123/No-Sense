using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawnScript : MonoBehaviour
{
    //public GameObject whitePiecePrefab;
    //public GameObject blackPiecePrefab;

    //private const int startYWhite = 0;
    //private const int endYWhite = 3;

    //private const int startYBlack = 5;
    //private const int endYBlack = 8;

    //public List<Piece> GenerateBoard(List<Piece> pieceList, ref GridKeeper gridKeeper)
    //{
    //    if (pieceList == null)
    //        pieceList = new List<Piece>();
    //    else pieceList.Clear();

    //    GenerateTeamBoard(pieceList, ref gridKeeper, Team.White, startYWhite, endYWhite);
    //    GenerateTeamBoard(pieceList, ref gridKeeper, Team.Black, startYBlack, endYBlack);

    //    return pieceList;
    //}

    //private void GenerateTeamBoard(List<Piece> pieceList, ref GridKeeper gridKeeper, Team team, int startY, int endY)
    //{
    //    GameObject prefab = team == Team.White ? whitePiecePrefab : blackPiecePrefab;
    //    for (int y = startY; y < endY; y++)
    //    {
    //        for (int x = 0; x < gridKeeper.Width; x++)
    //        {
    //            //First generate a piece
    //            //Piece piece = GeneratePiece(
    //            gridKeeper.SetData(y * gridKeeper.Width + x, team, true, false);
    //        }
    //    }
    //}

    //private Piece GeneratePiece(Location location, Team team)
    //{
    //    GameObject tempObject = Instantiate((team == Team.White) ? whitePiecePrefab : blackPiecePrefab);
    //    location.isOccupied = true;
    //    Piece tempPiece = tempObject.GetComponent<Piece>();
    //    tempPiece.location = location;
    //    tempPiece.team = team;
    //    location.piece = tempPiece;
    //    tempObject.transform.position = location.position;
    //    return tempPiece;
    //}
}
