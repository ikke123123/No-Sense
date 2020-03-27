using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LocationKeeper : MonoBehaviour
{
    [Header("Grid Spawning")]
    [SerializeField, Range(1, 15)] private int gridWidth = 1;
    [SerializeField, Range(1, 15)] private int gridDepth = 1;
    [SerializeField] private BoardSpawnScript boardSpawn = null;

    [Header("AI")]
    [SerializeField] private ThomasAI blackThomas = null;
    [SerializeField] private ThomasAI whiteThomas = null;
    //[SerializeField] private DeyvidAI blackDeyvid = null;
    //[SerializeField] private DeyvidAI whiteDeyvid = null;

    [Header("Win Text")]
    [SerializeField] private GameObject blackWins = null;
    [SerializeField] private GameObject whiteWins = null;

    [Header("Misc")]
    [SerializeField] private UnityEvent toDoWhenFinished = null;

    [HideInInspector] public Team turn = Team.White;
    [HideInInspector] public LocationGrid locationGrid = new LocationGrid();
    [HideInInspector] public List<Piece> whiteObjects;
    [HideInInspector] public List<Piece> blackObjects;
    [HideInInspector] public PossibleMove[] turnMoves;
    [HideInInspector] public bool restart = false;

    private float timeLeft = 6.0f;
    private float timer = 1;

    private void Awake()
    {
        locationGrid.GenerateGrid(gridWidth, gridDepth);
        if (boardSpawn != null)
        {
            whiteObjects = boardSpawn.GenerateBoard(locationGrid, Team.White);
            blackObjects = boardSpawn.GenerateBoard(locationGrid, Team.Black);
        }
        else Debug.LogWarning("Board Spawn wasn't assigned");
        turnMoves = CheckMoves(turn);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && restart == false)
        {
            if (turn == Team.Black) blackThomas.RequestDecision();
            else whiteThomas.RequestDecision();

            //if (turn == Team.Black)
            //{
            //    if (blackThomas == null) DeyvidAI.RequestDecision();
            //    else blackThomas.RequestDecision();
            //}
            //else
            //{
            //    if (whiteThomas == null) DeyvidAI.RequestDecision();
            //    else whiteThomas.RequestDecision();
            //}
            timer = 1;
        }
        if (restart == true)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                ResetMatch();
            }
        }
    }

    public void ExecuteMove(PossibleMove possibleMove)
    {
        if (possibleMove.strike) DestroyPiece(possibleMove.hitPiece);
        possibleMove.piece.MoveTo(possibleMove.toLocation);
        FollowedUp(possibleMove);
    }

    private bool FollowedUp(PossibleMove possibleMove)
    {
        if (((possibleMove.piece.team == Team.Black && possibleMove.piece.location.gridLocation.x == 0) || (possibleMove.piece.team == Team.White && possibleMove.piece.location.gridLocation.x == 7)) && possibleMove.piece.isSpecialPiece == false)
        {
            possibleMove.piece.MakeSpecialPiece();
        }
        List<PossibleMove> possibleMoves = new List<PossibleMove>();
        if (possibleMove.strike == true)
        {
            foreach (PossibleMove move in CheckMoves(turn))
            {
                if (move.strike == true && move.piece == possibleMove.piece)
                {
                    possibleMoves.Add(move);
                }
            }
            if (possibleMoves.Count > 0)
            {
                turnMoves = possibleMoves.ToArray();
                return true;
            }
        }
        turn = (turn == Team.Black ? Team.White : Team.Black);
        turnMoves = CheckMoves(turn);
        if (turnMoves.Length == 0)
        {
            EndGame(turn.ToString() + " has lost the game.");
        }
        return false;
    }

    public void EndGame(string input)
    {
        Debug.Log("Game has ended: " + input);
        if (turn == Team.Black)
        {
            whiteWins.SetActive(true);
        }
        else if (turn == Team.White) 
        {
            blackWins.SetActive(true);
        }
        toDoWhenFinished.Invoke();
        restart = true;
    }

    public void ResetMatch()
    {
        blackWins.SetActive(false);
        whiteWins.SetActive(false);
        timeLeft = 6;
        restart = false;
        foreach (Piece piece in whiteObjects) Destroy(piece.gameObject);
        foreach (Piece piece in blackObjects) Destroy(piece.gameObject);
        whiteObjects.Clear();
        blackObjects.Clear();
        locationGrid.GenerateGrid(gridWidth, gridDepth);
        if (boardSpawn != null)
        {
            whiteObjects = boardSpawn.GenerateBoard(locationGrid, Team.White);
            blackObjects = boardSpawn.GenerateBoard(locationGrid, Team.Black);
        }
        else Debug.LogWarning("Board Spawn wasn't assigned");
        turnMoves = CheckMoves(turn);
    }

    private void DestroyPiece(Piece piece)
    {
        piece.location.LeaveLocation();
        (piece.team == Team.Black ? blackObjects : whiteObjects).Remove(piece);
        Destroy(piece.gameObject);
    }

    private PossibleMove[] CheckMoves(Team team)
    {
        List<PossibleMove> possibleMoves = new List<PossibleMove>();

        bool strike = false;
        foreach (Piece piece in (team == Team.Black ? blackObjects : whiteObjects))
        {
            piece.possibleMoves.Clear();
            if (piece.isSpecialPiece == false)
            {
                for (int x = -1; x < 2; x += 2)
                {
                    //Check if it's a valid location
                    int tempX = x + piece.location.gridLocation.x;
                    if (ValidGridLocation(tempX, 0))
                    {
                        //Check if it's forward or backward
                        bool forward = (team == Team.Black && x == -1) || (team == Team.White && x == 1) ? true : false;

                        for (int y = -1; y < 2; y += 2)
                        {
                            //Check if it's a valid location
                            int tempY = y + piece.location.gridLocation.y;
                            if (ValidGridLocation(tempX, tempY))
                            {
                                Location tempLocation = locationGrid.locations[tempX, tempY];
                                if (strike == false && tempLocation.isOccupied == false && forward == true)
                                {
                                    PossibleMove possibleMove = new PossibleMove
                                    {
                                        piece = piece,
                                        fromLocation = piece.location,
                                        toLocation = tempLocation
                                    };
                                    possibleMoves.Add(possibleMove);
                                    piece.possibleMoves.Add(possibleMove);
                                }
                                if (tempLocation.isOccupied == true && OccupiedByFriendly(tempX, tempY, team) == false && PositionExistsAndIsFree(x + tempX, y + tempY))
                                {
                                    strike = true;
                                    PossibleMove possibleMove = new PossibleMove
                                    {
                                        piece = piece,
                                        fromLocation = piece.location,
                                        toLocation = locationGrid.locations[x + tempX, y + tempY],
                                        strike = true,
                                        hitPiece = tempLocation.piece
                                    };
                                    possibleMoves.Add(possibleMove);
                                    piece.possibleMoves.Add(possibleMove);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int x = -1; x < 2; x += 2)
                {
                    for (int y = -1; y < 2; y += 2)
                    {
                        //Check if it's a valid location
                        int tempX = x + piece.location.gridLocation.x;
                        int tempY = y + piece.location.gridLocation.y;
                        //Debug.Log(tempX + " " + tempY);
                        bool striking = false;
                        while (ValidGridLocation(tempX, tempY))
                        {
                            //Debug.Log(tempX + " " + tempY);
                            Location tempLocation = locationGrid.locations[tempX, tempY];
                            if (tempLocation.isOccupied == false)
                            {
                                PossibleMove possibleMove = new PossibleMove
                                {
                                    piece = piece,
                                    fromLocation = piece.location,
                                    toLocation = tempLocation
                                };
                                possibleMoves.Add(possibleMove);
                                piece.possibleMoves.Add(possibleMove);
                            }
                            if (tempLocation.isOccupied == true)
                            {
                                if (OccupiedByFriendly(tempX, tempY, team) || striking == true) break;
                                if (PositionExistsAndIsFree(x + tempX, y + tempY) == false) break;
                                int xUltraTemp = 0;
                                int yUltraTemp = 0;
                                while (PositionExistsAndIsFree(x + tempX + xUltraTemp, y + tempY + yUltraTemp))
                                {
                                    strike = true;
                                    striking = true;
                                    PossibleMove possibleMove = new PossibleMove
                                    {
                                        piece = piece,
                                        fromLocation = piece.location,
                                        toLocation = locationGrid.locations[x + tempX + xUltraTemp, y + tempY + yUltraTemp],
                                        strike = true,
                                        hitPiece = tempLocation.piece
                                    };
                                    possibleMoves.Add(possibleMove);
                                    piece.possibleMoves.Add(possibleMove);
                                    xUltraTemp += x;
                                    yUltraTemp += y;
                                }
                            }

                            tempX += x;
                            tempY += y;
                        }
                    }
                }
            }
        }
        //Clean if strike is true
        if (strike)
        {
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                if (possibleMoves[i].strike == false)
                {
                    possibleMoves[i].piece.possibleMoves.Remove(possibleMoves[i]);
                    possibleMoves.Remove(possibleMoves[i]);
                    i--;
                }
            }
        }
        return possibleMoves.ToArray();
    }

    public bool PositionExistsAndIsFree(int x, int y)
    {
        return ValidGridLocation(x, y) ? (locationGrid.locations[x, y].isOccupied ? false : true) : false;
    }

    public bool OccupiedByFriendly(int x, int y, Team team)
    {
        return (team == Team.Black ? blackObjects : whiteObjects).Contains(locationGrid.locations[x, y].piece);
    }

    public bool ValidGridLocation(int x, int y)
    {
        return (x < locationGrid.locations.GetLength(0) && x > -1) && (y < locationGrid.locations.GetLength(1) && y > -1);
    }
}

public class PossibleMove
{
    public Piece piece = null;
    public Location fromLocation = null;
    public Location toLocation = null;
    public bool strike = false;
    public Piece hitPiece;
}