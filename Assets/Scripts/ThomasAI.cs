using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThomasAI : MonoBehaviour
{
    [SerializeField] private LocationKeeper locationKeeper = null;
    [SerializeField] private Team team = Team.Black;

    //Yeah... This is gonna cost some performance.
    public PossibleMove RequestDecision(List<PossibleMove> possibleMoves, ref LocationGrid locationGrid)
    {
        return possibleMoves[Random.Range(0, possibleMoves.Count)];
    }
}