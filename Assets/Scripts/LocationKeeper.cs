using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;

public class LocationKeeper : MonoBehaviour
{
    public const int gridWidth = 8;
    public const int gridHeight = 8;

    public const int tileScale = 2;

    public readonly static Vector3 origin = new Vector3(-gridWidth * 0.5f * tileScale + 1, 1.25f, -gridHeight * 0.5f * tileScale + 1);

    public readonly static Vector2Int[] whiteMoveTable = new Vector2Int[]
    {
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1)
    };

    public readonly static Vector2Int[] blackMoveTable = new Vector2Int[]
    {
        new Vector2Int(1, 1),
        new Vector2Int(-1, 1)
    };

    public readonly static Vector2Int[] specialMoveTable = new Vector2Int[]
    {
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 1)
    };

    private List<Piece> TurnPieces => turn == Team.White ? whiteObjects : blackObjects;

    [Header("Grid Spawning")]
    [SerializeField] private float turnWaitTime;
    [SerializeField] private float endGameWaitTime;
    [SerializeField] private GameObject blackPrefab;
    [SerializeField] private GameObject whitePrefab;

    [Header("AI")]
    [SerializeField] private ThomasAI blackThomas = null;
    [SerializeField] private ThomasAI whiteThomas = null;

    [Header("Win Text")]
    [SerializeField] private GameObject blackWins = null;
    [SerializeField] private GameObject whiteWins = null;

    public Team turn = Team.White;
    public LocationGrid locationGrid = new LocationGrid();
    public List<Piece> whiteObjects;
    public List<Piece> blackObjects;
    public List<PossibleMove> turnMoves;
    public bool restart = false;

    private void Start()
    {
        Initialize();

        StartCoroutine(Ticker());
    }

    private void Initialize()
    {
        blackWins.SetActive(false);
        whiteWins.SetActive(false);

        restart = false;

        foreach (Piece piece in whiteObjects)
            Destroy(piece.gameObject);
        foreach (Piece piece in blackObjects)
            Destroy(piece.gameObject);

        whiteObjects.Clear();
        blackObjects.Clear();

        locationGrid = new LocationGrid(gridWidth, gridHeight);

        GenerateBoard(locationGrid, whiteObjects, 0, 3, Team.White, whitePrefab);
        GenerateBoard(locationGrid, blackObjects, 5, 8, Team.Black, blackPrefab);

        CheckMoves(turn, TurnPieces, turnMoves, ref locationGrid);
    }

    private IEnumerator Ticker()
    {
        while (true)
        {
            while (restart == false)
            {
                if (turn == Team.Black)
                    ExecuteMove(blackThomas.RequestDecision(turnMoves, ref locationGrid));
                else ExecuteMove(whiteThomas.RequestDecision(turnMoves, ref locationGrid));

                yield return new WaitForSeconds(turnWaitTime);
            }

            Initialize();

            yield return new WaitForSeconds(endGameWaitTime);
        }
    }

    public void GenerateBoard(LocationGrid locationGrid, List<Piece> list, int startY, int endY, Team team, GameObject prefab)
    {
        for (int y = startY; y < endY; y++)
        {
            for (int x = Location.GetXOffset(y); x < gridWidth; x += 2)
            {
                list.Add(GeneratePiece(locationGrid.locations[y, x], team, prefab));
            }
        }
    }

    private Piece GeneratePiece(Location location, Team team, GameObject prefab)
    {
        Piece tempPiece = Instantiate(prefab).GetComponent<Piece>();
        tempPiece.location = location;
        tempPiece.team = team;
        location.piece = tempPiece;

        tempPiece.transform.position = location.Position;

        return tempPiece;
    }

    public void ExecuteMove(PossibleMove move)
    {
        if (move.strike)
            DestroyPiece(move.hitPiece);
        move.piece.MoveTo(move.toLocation);
        FollowUp(move);
    }

    /// <summary>
    /// Returns true if the turn of the player who just moved is not over.
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    private bool FollowUp(PossibleMove move)
    {
        Piece piece = move.piece;

        //Make piece a special piece if it reaches y 7 or 0, depending on team.
        int setSpecialY = piece.team == Team.White ? 7 : 0;
        if (!piece.isSpecialPiece && piece.Position.y == setSpecialY)
            move.piece.MakeSpecialPiece();

        if (move.strike)
        {
            List<PossibleMove> tempMoves = new List<PossibleMove>();
            //This check here is so that it does not hand out the turn to the other team if there is one or more strikes possible in the same turn. PLEASE UPDATE SO THAT THIS ONE ONLY CHECKS FOR THIS OBJECT!!!!
            CheckMoves(turn, TurnPieces, tempMoves, ref locationGrid);
            turnMoves.Clear();

            foreach (PossibleMove tempMove in tempMoves)
            {
                if (tempMove.strike && tempMove.piece == move.piece)
                    turnMoves.Add(tempMove);
            }

            if (turnMoves.Count > 0)
                return true;
        }

        //Change whose turn it is.
        turn = (turn == Team.Black ? Team.White : Team.Black);

        CheckMoves(turn, TurnPieces, turnMoves, ref locationGrid);

        if (turnMoves.Count == 0)
            EndGame(turn.ToString() + " has lost the game.");

        return false;
    }

    public void EndGame(string input)
    {
        Debug.Log("Game has ended: " + input);
        if (turn == Team.Black)
            whiteWins.SetActive(true);
        else blackWins.SetActive(true);

        restart = true;
    }

    private void DestroyPiece(Piece piece)
    {
        piece.location.LeaveLocation();
        (piece.team == Team.Black ? blackObjects : whiteObjects).Remove(piece);
        Destroy(piece.gameObject);
    }

    public static void CheckMoves(Team team, List<Piece> pieces, List<PossibleMove> possibleMoves, ref LocationGrid locationGrid)
    {
        //Okay... I was going to redo this whole bit... But damn... I should have made more notes when I had the chance.
        //Never mind I redid it anyways, it's better, but still not good.

        if (possibleMoves == null)
            possibleMoves = new List<PossibleMove>();
        else possibleMoves.Clear();

        bool strike = false;

        Vector2Int[] teamMoveTable = team == Team.White ? whiteMoveTable : blackMoveTable;

        foreach (Piece piece in pieces)
            if (piece.isSpecialPiece)
                PossiblePieceMoves(piece, possibleMoves, ref strike, ref locationGrid, specialMoveTable);
            else PossiblePieceMoves(piece, possibleMoves, ref strike, ref locationGrid, teamMoveTable);

        //Clean possible moves if a strike has been found.
        if (strike)
            possibleMoves = possibleMoves.Where(move => move.strike).ToList();
    }

    /// <summary>
    /// Returns true if it can strike another object. Does NOT clear the moves list.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="allowedMoveDirections"></param>
    /// <returns></returns>
    public static bool PossiblePieceMoves(Piece piece, List<PossibleMove> moves, ref bool strike, ref LocationGrid locationGrid, params Vector2Int[] allowedMoveDirections)
    {
        foreach (Vector2Int moveDir in allowedMoveDirections)
        {
            Vector2Int newPos = piece.Position + moveDir;
            bool canGo = true;

            while (canGo && ValidGridLocation(newPos))
            {
                Location tempLocation = GetLocation(newPos, ref locationGrid);

                if (!tempLocation.IsOccupied)
                    if (!strike)
                        moves.Add(new PossibleMove(piece, tempLocation));
                    else if (tempLocation.piece.team != piece.team)
                    {
                        newPos += moveDir;
                        while (canGo && PositionExistsAndIsFree(newPos, ref locationGrid))
                        {
                            moves.Add(new PossibleMove(piece, tempLocation, true, tempLocation.piece));
                            strike = true;
                            canGo = piece.isSpecialPiece;
                            newPos += moveDir;
                        }
                    }

                newPos += moveDir;
                canGo = piece.isSpecialPiece;
            }
        }
        return strike;
    }

    public static bool PositionExistsAndIsFree(Vector2Int pos, ref LocationGrid locationGrid) => ValidGridLocation(pos) && !GetLocation(pos, ref locationGrid).IsOccupied;

    public static Location GetLocation(Vector2Int pos, ref LocationGrid locationGrid) => locationGrid.locations[pos.x, pos.y];

    public static bool ValidGridLocation(Vector2Int pos) => pos.x < gridWidth && pos.x > -1 && pos.y < gridHeight && pos.y > -1;
}

[System.Serializable]
public class PossibleMove
{
    public PossibleMove(Piece piece, Location toLocation, bool strike = false, Piece hitPiece = null)
    {
        this.piece = piece;
        this.fromLocation = piece.location;
        this.toLocation = toLocation;
        this.strike = strike;
        this.hitPiece = hitPiece;
    }

    public Piece piece;
    public Location fromLocation;
    public Location toLocation;
    public bool strike;
    public Piece hitPiece;
}