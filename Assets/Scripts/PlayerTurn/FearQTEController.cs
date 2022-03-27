﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearQTEController : MonoBehaviour
{
    private float timeLimit, timer, speed, barSpeed, yOffset;
    public float multiplierF;
    private bool hit;
    private FearQTEController _marker;
    public GameObject bar;
    public MyQTEEvent myEvent { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = 5f;
        timer = 0f;
        speed = 1f;
        barSpeed = 1f;
        yOffset = 1.5f;
        hit = false;
        _marker = GetComponent<FearQTEController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeLimit || hit)
        {
            multiplierF = ((_marker.transform.position.y + yOffset) / (3.6f*0.3f) ) + 1f; // may need adjustment
            myEvent.Invoke(4, multiplierF);
            //Debug.Log("Fear damage multiplier: " + multiplierF);
            //Debug.Log("Timer: " + timer + ", Hit: " + hit);
        }
        else
        {
            timer += Time.deltaTime;
            HandleMovement();
            HandleBarMove();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _marker.transform.Translate(1f * speed * Time.deltaTime, 0f, 0f, Space.World);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _marker.transform.Translate(-1f * speed * Time.deltaTime, 0f, 0f, Space.World);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _marker.transform.Translate(0f, 1f * speed * Time.deltaTime, 0f, Space.World);
        }
    }

    private void HandleBarMove()
    {
        if (timer >= 1.5f)
        {
            bar.transform.Translate(0f, 1f * barSpeed * Time.deltaTime, 0f, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D obstacle)
    {
        if (obstacle.gameObject == bar)
        {
            hit = true;
            //timer = timeLimit;
        }
    }
}
