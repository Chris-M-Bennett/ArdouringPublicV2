using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrideQTEController : MonoBehaviour
{
    private float distFromCrit, maxDist, timer, timeLimit, multiplierP, readingTime;
    private bool stop, tutorial;
    private PrideQTEController _marker;
    public GameObject critPoint;
    public GameObject prideRing;
    public MyQTEEvent myEvent { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = 5f;
        readingTime = 10f;
        timer = 0f;
        maxDist = 1.4f * 0.3f;
        stop = false;
        tutorial = false;
        _marker = GetComponent<PrideQTEController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!tutorial)
        {
            if (timer >= timeLimit || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || stop)
            {
                stop = true;
                distFromCrit = Vector2.Distance (_marker.transform.position, critPoint.transform.position);
                multiplierP = (((distFromCrit - maxDist) * -1) / maxDist) + 0.5f;
                myEvent.Invoke(3, multiplierP);
                //Debug.Log("Pride damage multiplier: " + multiplierP);
            }
            else
            {
                timer += Time.deltaTime;
                transform.RotateAround(prideRing.transform.position, Vector3.forward, 144 * Time.deltaTime);
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
}
