using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyQTEController : MonoBehaviour
{
    private float timer, timeLimit, multiplierJ;
    private int slottedWedgeCount;
    private JoyQTEController _marker;
    public GameObject WedgeNL, WedgeNR, WedgeEU, WedgeED, WedgeSR, WedgeSL, WedgeWD, WedgeWU;
    public GameObject JoyWheel;
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = 5f;
        timer = 0f;
        slottedWedgeCount = 0;
        _marker = GetComponent<JoyQTEController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeLimit || Input.GetKeyDown(KeyCode.E) || slottedWedgeCount == 8)
        {
            multiplierJ = ((slottedWedgeCount - 4) / 8) + 1f;
            Debug.Log("Joy damage multiplier: " + multiplierJ);
        }
        else
        {
            timer += Time.deltaTime;
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _marker.transform.RotateAround(JoyWheel.transform.position, Vector3.forward, 72 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _marker.transform.RotateAround(JoyWheel.transform.position, Vector3.back, 72 * Time.deltaTime);
        }
    }

    private void OnTriggerStay2D(Collider2D wedge)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
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

            slottedWedgeCount += 1;
        }
    }
}
