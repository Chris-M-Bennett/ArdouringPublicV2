using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerQteController : MonoBehaviour
{
    private float timeLimit, timer, markerY, upForce, dropSpeed;
    public float multiplierA;
    private AngerQteController _marker;
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = 5f;
        timer = 0f;
        upForce = 35f; //20f;
        dropSpeed = 1f;
        _marker = GetComponent<AngerQteController>();
        // marker start y = -1.65f, marker x = -0.25
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer >= timeLimit)
        {
            multiplierA = (_marker.transform.position.y / 3.3f) + 1f;
            //Debug.Log("Anger damage multiplier: " + multiplierA);
        }
        else
        {
            timer += Time.deltaTime;
            _marker.transform.Translate(0f, -1f * dropSpeed * Time.deltaTime, 0f, Space.World);
            if (Input.GetKeyDown(KeyCode.E))
            {
                _marker.transform.Translate(0f, 1f * upForce * Time.deltaTime, 0f, Space.World);
            }
        }
    }
}
