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

    public Dictionary<Vector2Int, Piece> positionDictionary = new Dictionary<Vector2Int, Piece>();

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

        positionDictionary.Clear();

        GenerateTeamBoard(pieceList, ref gridKeeper, Team.White, whitePiecePrefab, ref gridKeeper.whitePieces, startYWhite, endYWhite);
        GenerateTeamBoard(pieceList, ref gridKeeper, Team.Black, blackPiecePrefab, ref gridKeeper.blackPieces, startYBlack, endYBlack);

        return pieceList;
    }

    /// <summary>
    /// Yeah this is a bit long.
    /// </summary>
    /// <param name="gridKeeperPos">This is the int array that maintains the positions of all the pieces for easy access.</param>
    private void GenerateTeamBoard(List<Piece> pieceList, ref GridKeeper gridKeeper, Team team, GameObject prefab, ref Vector2Int[] gridKeeperPos, int startY, int endY)
    {
        //Number of pieces that must be fit into the array initially.
        gridKeeperPos = new Vector2Int[gridWidth * (endY - startY)];

        int j = 0;

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);

                //First generate a piece
                Piece newPiece = Instantiate(prefab).GetComponent<Piece>();
                pieceList.Add(newPiece);

                //Add it to the dictionary so we can access it later.
                positionDictionary.Add(gridPos, newPiece);

                //Set its position to an actual good position on the board.
                newPiece.transform.position = ToPosition(gridPos);

                //Refer to the position of the ones in the grid keeper.
                gridKeeperPos[j] = gridPos;
                j++;

                //Then set the data on the grid keeper.
                gridKeeper.SetData(gridPos, team, true, false);
            }
        }
    }

    private Vector3 ToPosition(Vector2Int position)
    {
        float x = position.x * 2 + (position.y + 1) % 2; //Calculate position, plus switching offset.
        return new Vector3(x * scale, 0, position.y * scale) + basePos;
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

        Vector2Int[] selectedPlayers = team == Team.White ? gridKeeper.whitePieces : gridKeeper.blackPieces; 

        for (int i = 0; i < selectedPlayers.Length; i++)
        {
            //Check possible movements for all pawns.

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
    public int startPos;
    public int endPos;
    public int strikePos;
}