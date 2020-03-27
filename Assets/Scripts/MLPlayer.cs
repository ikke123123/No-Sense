//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using MLAgents;

//public class MLPlayer : Agent
//{
//    [SerializeField] private Team team = Team.Black;
//    [SerializeField] private LocationKeeper locationKeeper = null;

//    private List<Piece> ownPieces;
//    private List<Piece> enemyPieces;
//    private int ownPiecesCount;
//    private int enemyPiecesCount;

//    public override void InitializeAgent()
//    {
//        ownPieces = team == Team.Black ? locationKeeper.blackObjects : locationKeeper.whiteObjects;
//        enemyPieces = team == Team.White ? locationKeeper.blackObjects : locationKeeper.whiteObjects;
//        ownPiecesCount = ownPieces.Count;
//        enemyPiecesCount = enemyPieces.Count;
//    }

//    public override void CollectObservations()
//    {
//        foreach (Piece piece in ownPieces)
//        {
//            AddVectorObs(piece.location.gridLocation);
//        }
//        foreach (Piece piece in enemyPieces)
//        {
//            AddVectorObs(piece.location.gridLocation);
//        }
//        foreach (PossibleMove possibleMove in locationKeeper.turnMoves)
//        {
//            AddVectorObs(possibleMove.fromLocation.gridLocation);
//            AddVectorObs(possibleMove.toLocation.gridLocation);
//        }
//        AddVectorObs(ownPiecesCount);
//        AddVectorObs(enemyPiecesCount);
//        AddVectorObs(ownPieces.Count);
//        AddVectorObs(enemyPieces.Count);
//        AddVectorObs(locationKeeper.turnMoves.Length);
//    }

//    public override void AgentAction(float[] vectorAction)
//    {
//        locationKeeper.ExecuteMove(locationKeeper.turnMoves[Mathf.Clamp(Mathf.RoundToInt(vectorAction[0]), 0, locationKeeper.turnMoves.Length)]);

//        if (vectorAction[0] < 0 || vectorAction[0] > locationKeeper.turnMoves.Length)
//        {
//            SetReward(-0.5f);
//        }
//        if (ownPiecesCount < ownPieces.Count)
//        {
//            SetReward(-10);
//            ownPiecesCount = ownPieces.Count;
//        }
//        if (enemyPiecesCount < enemyPieces.Count)
//        {
//            SetReward(20);
//            enemyPiecesCount = enemyPieces.Count;
//        }
//    }

//    public override void AgentReset()
//    {
//        if (locationKeeper.turn == Team.Black && team == Team.White || locationKeeper.turn == Team.White && team == Team.Black)
//        {
//            SetReward(50);
//        }
//    }

//    public override float[] Heuristic()
//    {
//        return new float[]
//        {
//            Input.GetAxis("Horizontal")
//        };
//    }
//}
