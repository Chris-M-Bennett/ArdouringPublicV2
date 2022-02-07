using System.Collections;
using System.Collections.Generic;
using Opponents;
using UnityEngine;

public class OpponentSpawnScript : MonoBehaviour
{
    [SerializeField] private GameObject liveOpponent;
    [SerializeField] private GameObject defeatedOpponent;
    [SerializeField, Tooltip("The movement point the opponent should by moving away from")] private DirectOverworldMovementScript lastDest;
    [SerializeField, Tooltip("The movement point the spawned live opponent should by moving towards")]
    private DirectOverworldMovementScript currentDest;
    [SerializeField] private Vector2 offSet;

    private int defeatState;

    private void Start()
    {
        if (defeatState > 0)
        {
            Instantiate(defeatedOpponent, transform);
        }
        else if (defeatState == 0)
        {
            var live = Instantiate(liveOpponent, transform);
            live.transform.Translate(offSet);
            var myComponent = live.GetComponent<OpponentOverworldScript>();
            myComponent.LastDest = lastDest;
            myComponent.CurrentDest = currentDest;
        }
    }
}
