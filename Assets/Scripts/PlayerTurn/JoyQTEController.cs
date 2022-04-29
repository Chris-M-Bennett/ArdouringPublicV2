using System;
using System.Collections;
using System.Collections.Generic;
using Opponents;
using Player;
using UnityEngine;

public class JoyQTEController : MonoBehaviour
{
    private float timer, timeLimit, multiplierJ, slottedWedgeCount, distX, distY, readingTime;
    private JoyQTEController _marker;
    //private PlayerDebateActionsScript _playerAction;
    //public GameObject WedgeNL, WedgeNR, WedgeEU, WedgeED, WedgeSR, WedgeSL, WedgeWD, WedgeWU;
    public GameObject JoyWheel, CurrentWedge;
    private bool tutorial;
    public MyQTEEvent myEvent { get; set; }
    // Start is called before the first frame update
    private static DebateValuesScript _opponentValues;
    void Start()
    {
        timeLimit = 5f;
        readingTime = 10f;
        timer = 0f;
        slottedWedgeCount = 0f;
        _marker = GetComponent<JoyQTEController>();
        //_playerAction = BattleCanvas.AddComponent<PlayerDebateActionsScript>();
        //myEvent = new MyQTEEvent();
        _opponentValues = GameObject.FindWithTag("Opponent").GetComponent<DebateValuesScript>();
        if (_opponentValues.debaterName == "Tutorial Goblin")
        {
            tutorial = true;
        }
        else
        {
            tutorial = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorial)
        {
            if (timer >= timeLimit || slottedWedgeCount == 8f)
            {
                multiplierJ = (slottedWedgeCount / 8f) + 0.5f;
                //tell PlayerDebateActionsScript what multiplierJ is
                //_playerAction.qteMultiplier = multiplierJ;
                //_playerAction.CheckPlayerTurn(0);
                myEvent.Invoke(0, multiplierJ);
                //Debug.Log("Joy damage multiplier: " + multiplierJ);
            }
            else
            {
                timer += Time.deltaTime;
                HandleMovement();
            }
        }
        else
        {
            timer += Time.deltaTime;
            //tool tip explaining the QTE
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) && timer >= readingTime) //button to unfreeze
            {
                timer = 0f;
                tutorial = false;
            }
        }    
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _marker.transform.RotateAround(JoyWheel.transform.position, Vector3.forward, 144 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _marker.transform.RotateAround(JoyWheel.transform.position, Vector3.back, 144 * Time.deltaTime);
        }
    }

    private void OnTriggerStay2D(Collider2D wedge)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            slottedWedgeCount += 1f;
            CurrentWedge = wedge.gameObject; /*
            if (JoyWheel.transform.position.x - CurrentWedge.transform.position.x > 0f)
            {
                distX = 0.4f;
            }
            else
            {
                distX = -0.4f;
            }
            if (JoyWheel.transform.position.y - CurrentWedge.transform.position.y > 0f)
            {
                distY = 0.4f;
            }
            else
            {
                distY = -0.4f;
            } */
            CurrentWedge.transform.Translate((JoyWheel.transform.position.x - CurrentWedge.transform.position.x)/3f, (JoyWheel.transform.position.y - CurrentWedge.transform.position.y)/3f, 0f, Space.World);
            /*
            if (wedge.gameObject == WedgeNL)
            {
                WedgeNL.transform.position = new Vector2 (-0.5f, 0.9f);
            }
            if (wedge.gameObject == WedgeNR)
            {
                WedgeNR.transform.position = new Vector2 (0.5f, 0.9f);
            }
            if (wedge.gameObject == WedgeEU)
            {
                WedgeEU.transform.position = new Vector2 (0.9f, 0.5f);
            }
            if (wedge.gameObject == WedgeED)
            {
                WedgeED.transform.position = new Vector2 (0.9f, -0.5f);
            }
            if (wedge.gameObject == WedgeSR)
            {
                WedgeSR.transform.position = new Vector2 (0.5f, -0.9f);
            }
            if (wedge.gameObject == WedgeSL)
            {
                WedgeSL.transform.position = new Vector2 (-0.5f, -0.9f);
            }
            if (wedge.gameObject == WedgeWD)
            {
                WedgeWD.transform.position = new Vector2 (-0.9f, -0.5f);
            }
            if (wedge.gameObject == WedgeWU)
            {
                WedgeWU.transform.position = new Vector2 (-0.9f, 0.5f);
            }
            */
        }
    }
}
