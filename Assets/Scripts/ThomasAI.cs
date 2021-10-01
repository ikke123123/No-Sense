﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThomasAI : MonoBehaviour
{
    //[SerializeField] private LocationKeeper locationKeeper = null;
    //[SerializeField] private Team team = Team.Black;

    //private List<Piece> enemyPieces;
    //private List<Piece> friendlyPieces;

    //private void Start()
    //{
    //    enemyPieces = (team == Team.White) ? locationKeeper.blackObjects : locationKeeper.whiteObjects;
    //    friendlyPieces = (team == Team.Black) ? locationKeeper.blackObjects : locationKeeper.whiteObjects;
    //}

    //public void RequestDecision(List<PossibleMove> possibleMoves)
    //{
    //    List<PossibleMove> badMoves = new List<PossibleMove>();
    //    List<PossibleMove> goodMoves = new List<PossibleMove>();

    //    if (friendlyPieces.Count > 1 || friendlyPieces.Count == 1 && enemyPieces.Count < 3)
    //    {
    //        foreach (PossibleMove possibleMove in possibleMoves)
    //        {
    //            if (CheckIfStrickenNextTurn(enemyPieces, possibleMove.toLocation, team == Team.Black ? Team.White : Team.Black, possibleMove).Length > 0 && possibleMove.strike == false)
    //            {
    //                badMoves.Add(possibleMove);
    //            }
    //            else goodMoves.Add(possibleMove);
    //        }
    //        PossibleMove greatBadMove = ReturnGreatestBadOption(badMoves);
    //        PossibleMove greatMove = ReturnGreatestOption(goodMoves.ToArray());

    //        //Execute move
    //        locationKeeper.ExecuteMove(greatMove != null ? greatMove : (goodMoves.Count > 0 ? goodMoves[Random.Range(0, goodMoves.Count)] : (greatBadMove != null ? greatBadMove : badMoves[Random.Range(0, badMoves.Count)])));
    //    }
    //    else
    //    {
    //        locationKeeper.ExecuteMove(possibleMoves[Random.Range(0, possibleMoves.Length)]);
    //    }
    //}

    //private PossibleMove ReturnGreatestOption(PossibleMove[] goodMoves)
    //{
    //    foreach (PossibleMove possibleMove in goodMoves)
    //    {
    //        if (possibleMove.strike && CheckIfStrickenNextTurn(friendlyPieces, possibleMove.toLocation, team, possibleMove).Length > 1)
    //        {
    //            return possibleMove;
    //        }
    //    }
    //    foreach (PossibleMove possibleMove in goodMoves)
    //    {
    //        foreach (Piece piece in friendlyPieces)
    //        {
    //            foreach (PossibleMove possibleMove1 in CheckIfStrickenNextTurn(enemyPieces, piece.location, team == Team.Black ? Team.White : Team.Black, possibleMove))
    //            {
    //                if (possibleMove1.strike && possibleMove1.toLocation.gridLocation == possibleMove.toLocation.gridLocation)
    //                {
    //                    return possibleMove;
    //                }
    //            }
    //        }
    //    }
    //    foreach (PossibleMove possibleMove in goodMoves)
    //    {
    //        if (possibleMove.toLocation.gridLocation.x == (team == Team.Black ? 0 : 7) && possibleMove.piece.isSpecialPiece == false)
    //        {
    //            return possibleMove;
    //        }
    //    }
    //    foreach (PossibleMove possibleMove in goodMoves)
    //    {
    //        if (possibleMove.toLocation.gridLocation.y == 0 || possibleMove.toLocation.gridLocation.y == 7)
    //        {
    //            return possibleMove;
    //        }
    //    }
    //    return null;
    //}

    //private PossibleMove ReturnGreatestBadOption(List<PossibleMove> badMoves)
    //{
    //    PossibleMove greatestOption = null;
    //    List<PossibleMove> reallyBadPossibleMoves = new List<PossibleMove>();
    //    //Run for every bad move
    //    foreach (PossibleMove possibleMove in badMoves)
    //    {
    //        //Check for end locations of enemy strikes on friendly pieces
    //        foreach (PossibleMove possibleMove1 in CheckIfStrickenNextTurn(enemyPieces, possibleMove.toLocation, team == Team.Black ? Team.White : Team.Black, possibleMove))
    //        {
    //            //Check for possible counters to that strike
    //            foreach (PossibleMove possibleMove2 in CheckIfStrickenNextTurn(friendlyPieces, possibleMove1.toLocation, team, possibleMove))
    //            {
    //                greatestOption = possibleMove;
    //            }
    //            if (CheckIfStrickenNextTurn(enemyPieces, possibleMove1.toLocation, team == Team.Black ? Team.White : Team.Black).Length > 1)
    //            {
    //                reallyBadPossibleMoves.Add(possibleMove);
    //            }
    //        }
    //    }
    //    if (badMoves.Count > reallyBadPossibleMoves.Count)
    //    {
    //        foreach (PossibleMove reallyBadPossibleMove in reallyBadPossibleMoves)
    //        {
    //            badMoves.Remove(reallyBadPossibleMove);
    //        }
    //    }
    //    return greatestOption;
    //}

    //private PossibleMove[] CheckIfStrickenNextTurn(List<Piece> list, Location location, Team team, PossibleMove possibleMoveFrom = null)
    //{
    //    List<PossibleMove> possibleMoves = new List<PossibleMove>();

    //    foreach (Piece piece in list)
    //    {
    //        if (piece.isSpecialPiece == false)
    //        {
    //            for (int x = -1; x < 2; x += 2)
    //            {
    //                //Check if it's a valid location
    //                int tempX = x + piece.location.gridLocation.x;
    //                if (locationKeeper.ValidGridLocation(tempX, 0))
    //                {
    //                    //Check if it's forward or backward
    //                    for (int y = -1; y < 2; y += 2)
    //                    {
    //                        //Check if it's a valid location
    //                        int tempY = y + piece.location.gridLocation.y;
    //                        if (locationKeeper.ValidGridLocation(tempX, tempY))
    //                        {
    //                            Location tempLocation = locationKeeper.locationGrid.locations[tempX, tempY];
    //                            if (tempLocation == location && locationKeeper.OccupiedByFriendly(tempX, tempY, team) == false && (locationKeeper.PositionExistsAndIsFree(x + tempX, y + tempY) || possibleMoveFrom == null ? possibleMoveFrom.fromLocation == locationKeeper.locationGrid.locations[x + tempX, y + tempY] : false))
    //                            {
    //                                PossibleMove possibleMove = new PossibleMove
    //                                {
    //                                    piece = piece,
    //                                    fromLocation = piece.location,
    //                                    toLocation = locationKeeper.locationGrid.locations[x + tempX, y + tempY],
    //                                    strike = true,
    //                                    hitPiece = tempLocation.piece
    //                                };
    //                                possibleMoves.Add(possibleMove);
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
    //                    int tempX = x + piece.location.gridLocation.x;
    //                    int tempY = y + piece.location.gridLocation.y;
    //                    bool striking = false;
    //                    while (locationKeeper.ValidGridLocation(tempX, tempY))
    //                    {
    //                        Location tempLocation = locationKeeper.locationGrid.locations[tempX, tempY];
    //                        if (tempLocation == location)
    //                        {
    //                            if (locationKeeper.OccupiedByFriendly(tempX, tempY, team) || striking == true) break;
    //                            if (locationKeeper.PositionExistsAndIsFree(x + tempX, y + tempY) == false) break;
    //                            int xUltraTemp = 0;
    //                            int yUltraTemp = 0;
    //                            while (locationKeeper.PositionExistsAndIsFree(x + tempX + xUltraTemp, y + tempY + yUltraTemp))///|| ((locationKeeper.ValidGridLocation(x + tempX + xUltraTemp, y + tempY + xUltraTemp) && (possibleMoveFrom == null ? possibleMoveFrom.fromLocation == locationKeeper.locationGrid.locations[x + tempX + xUltraTemp, y + tempY + xUltraTemp] : false))))
    //                            {
    //                                PossibleMove possibleMove = new PossibleMove
    //                                {
    //                                    piece = piece,
    //                                    fromLocation = piece.location,
    //                                    toLocation = locationKeeper.locationGrid.locations[x + tempX, y + tempY],
    //                                    strike = true,
    //                                    hitPiece = tempLocation.piece
    //                                };
    //                                possibleMoves.Add(possibleMove);
    //                                striking = true;
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
    //    return possibleMoves.ToArray();
    //}
}