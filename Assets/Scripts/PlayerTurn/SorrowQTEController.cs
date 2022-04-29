using System.Collections;
using System.Collections.Generic;
using Opponents;
using UnityEngine;

public class SorrowQTEController : MonoBehaviour
{
    private float timeLimit, timer, speed, cloudSpeed, cloudStrafe, csCooldown, cloudDirection, barFillRate, readingTime;
    public float multiplierS;
    private SorrowQTEController _marker;
    public GameObject TearDrop;
    public GameObject RainCloud;
    private bool tutorial;
    public MyQTEEvent myEvent { get; set; }
    // Start is called before the first frame update
    private static DebateValuesScript _opponentValues;
    void Start()
    {
        timeLimit = 5f;
        readingTime = 10f;
        timer = 0f;
        speed = 1.2f;
        cloudSpeed = 0.9f;
        csCooldown = 0f;
        cloudDirection = 1f;
        barFillRate = 0.228f;// 0.76f;
        _marker = GetComponent<SorrowQTEController>();
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
            if (timer >= timeLimit)
            {
                multiplierS = ((TearDrop.transform.position.y + 1.5f) / (3.8f *0.3f)) + 1f; // may need adjustment
                myEvent.Invoke(1, multiplierS);
                //Debug.Log("Sorrow damage multiplier: " + multiplierS);
            }
            else
            {
                timer += Time.deltaTime;
                HandleStrafing();
                HandleCloudMovement();
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

    private void HandleStrafing()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _marker.transform.Translate(1f * speed * Time.deltaTime, 0f, 0f, Space.World);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _marker.transform.Translate(-1f * speed * Time.deltaTime, 0f, 0f, Space.World);
        }
    }

    private void HandleCloudMovement()
    {

        if (RainCloud.transform.position.x + 3f >= (2.85f * 0.3f)) 
        {
            cloudDirection = -1f;
        }
        if (RainCloud.transform.position.x + 3f <= (-3.45f * 0.3f))
        {
            cloudDirection = 1f;
        }
        csCooldown += Time.deltaTime;
        if (csCooldown >= 0.25f)
        {
            csCooldown = 0f;
            cloudStrafe = Random.Range(-1.5f, 0.9f);
            //cloudStrafe *= -1f;
        }
        RainCloud.transform.Translate(cloudDirection * (cloudSpeed + cloudStrafe) * Time.deltaTime, 0f, 0f, Space.World);
    }
    
    private void OnTriggerStay2D(Collider2D water)
    {
        if (water.gameObject == RainCloud && TearDrop.transform.position.y <= (1.9f * 0.3f) +1.5f)
        {
           TearDrop.transform.Translate(0f, 1f * barFillRate * Time.deltaTime, 0f, Space.World);
        }
    }
}
