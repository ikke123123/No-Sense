using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("AI")]
    [SerializeField] private ThomasAI black = null;
    [SerializeField] private ThomasAI white = null;

    [Header("Win Text")]
    [SerializeField] private GameObject blackWins = null;
    [SerializeField] private GameObject whiteWins = null;

    [Header("Misc")]
    [SerializeField] private UnityEvent toDoWhenFinished = null;
    [SerializeField] private float turnTime = 1;
    [SerializeField] private float timeBetweenMatches = 6;

    [HideInInspector] public bool restart = false;

    private LocationKeeper locationKeeper = null;

    private Coroutine ticker = null;

    private void Start()
    {
        locationKeeper = GetComponent<LocationKeeper>();
        locationKeeper.Initiate();

        //ticker = StartCoroutine(Tick());
    }

    //public IEnumerator Tick()
    //{
    //    while (true)
    //    {
    //        while (restart == false)
    //        {
    //            CheckMoves(turnMoves, currentTurn);
    //            yield return new WaitForSeconds(turnTime);
    //            if (currentTurn == Team.Black)
    //                black.RequestDecision(turnMoves);
    //            else white.RequestDecision(turnMoves);
    //        }
    //        yield return new WaitForSeconds(timeBetweenMatches);
    //        ResetMatch();
    //    }
    //}

    //public void ExecuteMove(PossibleMove possibleMove)
    //{
    //    if (possibleMove.strike) DestroyPiece(possibleMove.hitPiece);
    //    possibleMove.piece.MoveTo(possibleMove.toLocation);
    //    FollowedUp(possibleMove);
    //}

    //private bool FollowedUp(PossibleMove possibleMove)
    //{
    //    if (((possibleMove.piece.team == Team.Black && possibleMove.piece.location.gridLocation.x == 0) || (possibleMove.piece.team == Team.White && possibleMove.piece.location.gridLocation.x == 7)) && possibleMove.piece.isSpecialPiece == false)
    //    {
    //        possibleMove.piece.MakeSpecialPiece();
    //    }
    //    List<PossibleMove> possibleMoves = new List<PossibleMove>();
    //    if (possibleMove.strike == true)
    //    {
    //        foreach (PossibleMove move in CheckMoves(turnMoves, currentTurn))
    //        {
    //            if (move.strike == true && move.piece == possibleMove.piece)
    //            {
    //                possibleMoves.Add(move);
    //            }
    //        }
    //        if (possibleMoves.Count > 0)
    //        {
    //            turnMoves = possibleMoves;
    //            return true;
    //        }
    //    }
    //    currentTurn = (currentTurn == Team.Black ? Team.White : Team.Black);
    //    CheckMoves(turnMoves, currentTurn);
    //    if (turnMoves.Count == 0)
    //    {
    //        EndGame(currentTurn.ToString() + " has lost the game.");
    //    }
    //    return false;
    //}

    //public void EndGame(string input)
    //{
    //    Debug.Log("Game has ended: " + input);
    //    if (currentTurn == Team.Black)
    //    {
    //        whiteWins.SetActive(true);
    //    }
    //    else if (currentTurn == Team.White)
    //    {
    //        blackWins.SetActive(true);
    //    }
    //    toDoWhenFinished.Invoke();
    //    restart = true;
    //}

    //public void ResetMatch()
    //{
    //    blackWins.SetActive(false);
    //    whiteWins.SetActive(false);

    //    restart = false;
    //    foreach (Piece piece in whiteObjects) Destroy(piece.gameObject);
    //    foreach (Piece piece in blackObjects) Destroy(piece.gameObject);
    //    whiteObjects.Clear();
    //    blackObjects.Clear();
    //    locationGrid.GenerateGrid(gridWidth, gridHeight);
    //    if (boardSpawn != null)
    //    {
    //        whiteObjects = boardSpawn.GenerateBoard(locationGrid, Team.White);
    //        blackObjects = boardSpawn.GenerateBoard(locationGrid, Team.Black);
    //    }
    //    else Debug.LogWarning("Board Spawn wasn't assigned");
    //    CheckMoves(turnMoves, currentTurn);
    //}

    //private void DestroyPiece(Piece piece)
    //{
    //    piece.location.LeaveLocation();
    //    (piece.team == Team.Black ? blackObjects : whiteObjects).Remove(piece);
    //    Destroy(piece.gameObject);
    //}

    //private List<PossibleMove> CheckMoves(List<PossibleMove> possibleMoves, Team team)
    //{
    //    if (possibleMoves == null)
    //        possibleMoves = new List<PossibleMove>();
    //    else possibleMoves.Clear();

    //    bool strike = false;
    //    foreach (Piece piece in (team == Team.Black ? blackObjects : whiteObjects))
    //    {
    //        piece.possibleMoves.Clear();
    //        if (piece.isSpecialPiece == false)
    //        {
    //            for (int x = -1; x < 2; x += 2)
    //            {
    //                //Check if it's a valid location
    //                int tempX = x + piece.location.gridLocation.x;
    //                if (ValidGridLocation(tempX, 0))
    //                {
    //                    //Check if it's forward or backward
    //                    bool forward = (team == Team.Black && x == -1) || (team == Team.White && x == 1) ? true : false;

    //                    for (int y = -1; y < 2; y += 2)
    //                    {
    //                        //Check if it's a valid location
    //                        int tempY = y + piece.location.gridLocation.y;
    //                        if (ValidGridLocation(tempX, tempY))
    //                        {
    //                            Location tempLocation = locationGrid.locations[tempX, tempY];
    //                            if (strike == false && tempLocation.isOccupied == false && forward == true)
    //                            {
    //                                PossibleMove possibleMove = new PossibleMove
    //                                {
    //                                    piece = piece,
    //                                    fromLocation = piece.location,
    //                                    toLocation = tempLocation
    //                                };
    //                                possibleMoves.Add(possibleMove);
    //                                piece.possibleMoves.Add(possibleMove);
    //                            }
    //                            if (tempLocation.isOccupied == true && OccupiedByFriendly(tempX, tempY, team) == false && PositionExistsAndIsFree(x + tempX, y + tempY))
    //                            {
    //                                strike = true;
    //                                PossibleMove possibleMove = new PossibleMove
    //                                {
    //                                    piece = piece,
    //                                    fromLocation = piece.location,
    //                                    toLocation = locationGrid.locations[x + tempX, y + tempY],
    //                                    strike = true,
    //                                    hitPiece = tempLocation.piece
    //                                };
    //                                possibleMoves.Add(possibleMove);
    //                                piece.possibleMoves.Add(possibleMove);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int x = -1; x < 2; x += 2)
    //            {
    //                for (int y = -1; y < 2; y += 2)
    //                {
    //                    //Check if it's a valid location
    //                    int tempX = x + piece.location.gridLocation.x;
    //                    int tempY = y + piece.location.gridLocation.y;
    //                    //Debug.Log(tempX + " " + tempY);
    //                    bool striking = false;
    //                    while (ValidGridLocation(tempX, tempY))
    //                    {
    //                        //Debug.Log(tempX + " " + tempY);
    //                        Location tempLocation = locationGrid.locations[tempX, tempY];
    //                        if (tempLocation.isOccupied == false)
    //                        {
    //                            PossibleMove possibleMove = new PossibleMove
    //                            {
    //                                piece = piece,
    //                                fromLocation = piece.location,
    //                                toLocation = tempLocation
    //                            };
    //                            possibleMoves.Add(possibleMove);
    //                            piece.possibleMoves.Add(possibleMove);
    //                        }
    //                        if (tempLocation.isOccupied == true)
    //                        {
    //                            if (OccupiedByFriendly(tempX, tempY, team) || striking == true) break;
    //                            if (PositionExistsAndIsFree(x + tempX, y + tempY) == false) break;
    //                            int xUltraTemp = 0;
    //                            int yUltraTemp = 0;
    //                            while (PositionExistsAndIsFree(x + tempX + xUltraTemp, y + tempY + yUltraTemp))
    //                            {
    //                                strike = true;
    //                                striking = true;
    //                                PossibleMove possibleMove = new PossibleMove
    //                                {
    //                                    piece = piece,
    //                                    fromLocation = piece.location,
    //                                    toLocation = locationGrid.locations[x + tempX + xUltraTemp, y + tempY + yUltraTemp],
    //                                    strike = true,
    //                                    hitPiece = tempLocation.piece
    //                                };
    //                                possibleMoves.Add(possibleMove);
    //                                piece.possibleMoves.Add(possibleMove);
    //                                xUltraTemp += x;
    //                                yUltraTemp += y;
    //                            }
    //                        }

    //                        tempX += x;
    //                        tempY += y;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    //Clean if strike is true
    //    if (strike)
    //    {
    //        for (int i = 0; i < possibleMoves.Count; i++)
    //        {
    //            if (possibleMoves[i].strike == false)
    //            {
    //                possibleMoves[i].piece.possibleMoves.Remove(possibleMoves[i]);
    //                possibleMoves.Remove(possibleMoves[i]);
    //                i--;
    //            }
    //        }
    //    }
    //    return possibleMoves;
    //}

    //public bool PositionExistsAndIsFree(int x, int y)
    //{
    //    return ValidGridLocation(x, y) ? (locationGrid.locations[x, y].isOccupied ? false : true) : false;
    //}

    //public bool OccupiedByFriendly(int x, int y, Team team)
    //{
    //    return (team == Team.Black ? blackObjects : whiteObjects).Contains(locationGrid.locations[x, y].piece);
    //}

    //public bool ValidGridLocation(int x, int y)
    //{
    //    return (x < locationGrid.locations.GetLength(0) && x > -1) && (y < locationGrid.locations.GetLength(1) && y > -1);
    //}

}
