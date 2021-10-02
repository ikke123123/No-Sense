using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LocationKeeper : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;

    [HideInInspector] public Team currentTurn = Team.White;

    [HideInInspector] public GridKeeper locationGrid;

    [HideInInspector] public List<Piece> pieces;
    [HideInInspector] public List<PossibleMove> turnMoves;

    public Dictionary<int, Piece> positionDictionary = new Dictionary<int, Piece>();

    private const int gridWidth = 4;
    private const int gridHeight = 8;
    private const int scale = 2;

    private Vector3 basePos = new Vector3(-scale * 4 + 1, 1.25f, -scale * 4 + 1); //Position at which the board starts.

    private const int startYWhite = 0;
    private const int endYWhite = 3;

    private const int startYBlack = 5;
    private const int endYBlack = 8;

    public void Initiate()
    {
        locationGrid = new GridKeeper(gridWidth, gridHeight);

        GenerateBoard(pieces, ref locationGrid);
    }

    public List<Piece> GenerateBoard(List<Piece> pieceList, ref GridKeeper gridKeeper)
    {
        if (pieceList == null)
            pieceList = new List<Piece>();
        else pieceList.Clear();

        GenerateTeamBoard(pieceList, ref gridKeeper, Team.White, startYWhite, endYWhite);
        GenerateTeamBoard(pieceList, ref gridKeeper, Team.Black, startYBlack, endYBlack);

        return pieceList;
    }

    private void GenerateTeamBoard(List<Piece> pieceList, ref GridKeeper gridKeeper, Team team, int startY, int endY)
    {
        GameObject prefab = team == Team.White ? whitePiecePrefab : blackPiecePrefab;

        for (int i = startY * gridWidth; i < endY * gridWidth; i++)
        {
            //First generate a piece
            Piece newPiece = Instantiate(prefab).GetComponent<Piece>();
            pieceList.Add(newPiece);
            positionDictionary.Add(i, newPiece);
            newPiece.transform.position = ToPosition(i);

            //Then set the data on the grid keeper.
            gridKeeper.SetData(i, team, true, false);
        }
    }

    private Vector3 ToPosition(int position)
    {
        float y = Mathf.Floor(position / gridWidth);
        float x = position % gridWidth * 2 + (y + 1) % 2; //Calculate position, plus switching offset.
        return new Vector3(x * scale, 0, y * scale) + basePos;
    }

    private void PossibleMoves(List<PossibleMove> possibleMoves, ref GridKeeper gridKeeper, Team team)
    {
        //Move directions for non-special items are:
        //-9 and -8 and -7 for white, and 7 and 8 and 9 for black depending on line.
        //Move directions for special items are:
        //-9, -8, 8, and 9 for all.

        if (possibleMoves == null)
            possibleMoves = new List<PossibleMove>();
        else possibleMoves.Clear();

        int[] moveDirections = new int[2];
        for (int i = 0; i < gridHeight; i++)
        {
            
        }
    }
}

//public class PossibleMove
//{
//    public Piece piece = null;
//    public Vector2Int fromLocation;
//    public Vector2Int toLocation;
//    public bool strike = false;
//    public Piece hitPiece;
//}

public struct PossibleMove
{
    public LocationKeeper resultingLocationKeeper;
    public int score;
    public int startPos;
    public int endPos;
    public int strikePos;
}