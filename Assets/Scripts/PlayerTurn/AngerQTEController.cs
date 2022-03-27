using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class AngerQTEController : MonoBehaviour
{
    private float timeLimit, timer, markerY, upForce, dropSpeed, barScale, yOffset;
    public float multiplierA;
    private AngerQTEController _marker;
    public GameObject Bar;
    public MyQTEEvent myEvent { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = 5f;
        timer = 0f;
        upForce = 6f;
        dropSpeed = 0.3f;
        yOffset = 1.5f;
        //barScale = Bar.transform.localScale.y;
        //barScale = 0f;
        _marker = GetComponent<AngerQTEController>();
        // marker start y = -1.65f, marker x = -0.25
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer >= timeLimit)
        {
            multiplierA = (_marker.transform.position.y / (3.3f *0.3f)) + 1f + yOffset;
            myEvent.Invoke(2, multiplierA);
            //Debug.Log("Anger damage multiplier: " + multiplierA);
        }
        else
        {
            timer += Time.deltaTime;
            _marker.transform.Translate(0f, -1f * dropSpeed * Time.deltaTime, 0f, Space.World);
            
            barScale = (_marker.transform.position.y / (3.3f *0.3f)) + 0.5f + yOffset;
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                _marker.transform.Translate(0f, 1f * upForce * Time.deltaTime, 0f, Space.World);
            }
            Bar.transform.localScale = new Vector3(10f,10 * barScale,1f);
            //Bar.transform.position = new Vector3(Bar.transform.position.x,((-2.2f *0.3f) + yOffset) + ((2.2f *0.3f) * barScale),0f);
            Bar.transform.position = new Vector3(Bar.transform.position.x,-2.16f + (0.66f * barScale), 0f);
            // -2.2f + (2.2f * barScale)
            // -2.16f + (0.66f * barScale)
        }
    }
}
