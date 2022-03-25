using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorrowQTEController : MonoBehaviour
{
    private float timeLimit, timer, speed, cloudSpeed, cloudStrafe, csCooldown, cloudDirection, barFillRate;
    public float multiplierS;
    private SorrowQTEController _marker;
    public GameObject TearDrop;
    public GameObject RainCloud;
    public MyQTEEvent myEvent { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = 5f;
        timer = 0f;
        speed = 1.2f;
        cloudSpeed = 0.9f;
        csCooldown = 0f;
        cloudDirection = 1f;
        barFillRate = 0.76f;
        _marker = GetComponent<SorrowQTEController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeLimit)
        {
            multiplierS = (TearDrop.transform.position.y / 3.8f) + 1f; // may need adjustment
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
        
        if (RainCloud.transform.position.x >= 2.85f)
        {
            cloudDirection = -1f;
        }
        if (RainCloud.transform.position.x <= -3.45f)
        {
            cloudDirection = 1f;
        }
        csCooldown += Time.deltaTime;
        if (csCooldown >= 0.25f)
        {
            csCooldown = 0f;
            cloudStrafe = Random.Range(-5f, 3f);
            //cloudStrafe *= -1f;
        }
        RainCloud.transform.Translate(cloudDirection * (cloudSpeed + cloudStrafe) * Time.deltaTime, 0f, 0f, Space.World);
    }
    
    private void OnTriggerStay2D(Collider2D water)
    {
        if (water.gameObject == RainCloud && TearDrop.transform.position.y <= 1.9f)
        {
           TearDrop.transform.Translate(0f, 1f * barFillRate * Time.deltaTime, 0f, Space.World);
        }
    }
}
