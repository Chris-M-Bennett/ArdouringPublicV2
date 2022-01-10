using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Opponents;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float posX;
    private float posY;
    public float xStrafe;
    private float farRightX;
    private float farLeftX;
    private float quitTimer;
    private float barX;
    private float barY;
    private float esDrop;
    public int maxES = 100;
    public int currentES = 100;
    public GameObject Player;
    //private GameObject player;
    public GameObject text;
    public GameObject ESbar;
    //private DebateValuesScript _playerValues;
    
    // Start is called before the first frame update
    void Start()
    {
        posX = -3.25f;
        posY = -2.06f;
        xStrafe = 0.33f;
        farRightX = posX - 0.01f + 2 * xStrafe;
        farLeftX = posX + 0.01f - 2 * xStrafe;
        Player.transform.position = new Vector2 (posX, posY);
        //_playerValues = player.GetComponent<DebateValuesScript>();
        maxES = 100;
        currentES = 100;
        //currentES = _playerValues.currentES;  //maxES;
       // Debug.Log("CurrentES: " + currentES);
        esDrop = 0.133f;
        barX = -2.335f;
        barY = -1.565f; //- (esDrop * (maxES - currentES));
        //HandleBarDrop();
        
        
        ESbar.transform.position = new Vector2 (barX, barY);
        
        // debug messages I used to figure out why the player could move off the left side of the screen instead of stopping.
        Debug.Log("Right strafe limit: " + farRightX);
        Debug.Log("Left strafe limit: " + farLeftX);
        Debug.Log("X strafe amount: " + xStrafe); //*/
    }

    // Update is called once per frame
    void Update()
    {
        if (currentES > 0)
        {
            HandleStrafe();
        }
        else
        {
            if (quitTimer == 0)
            {
                GameObject note = Instantiate(text,new Vector2(2f,0f),Quaternion.identity);
                Debug.Log("Game Over");
            }
            GameOver();
        }
    }

    private void HandleStrafe()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && posX < farRightX)
        {
            posX += xStrafe;
            Player.transform.position = new Vector2 (posX, posY);
            //Debug.Log("Player X: " + posX);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && posX > farLeftX) //this was originally posX != farLeftX but due to floating point arithmetic farLextX is 0.67999999999999 etc instead of 0.68 so the > circumvents this.
        {
            posX -= xStrafe;
            Player.transform.position = new Vector2 (posX, posY);
            //Debug.Log("Player X: " + posX);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D hit)
    {
        
        if (hit.gameObject.tag == "Bullet")
        {
            currentES -= hit.GetComponent<BulletController>().damage;
            barY -= esDrop;
            ESbar.transform.position = new Vector2 (barX, barY);
            Debug.Log("Emotional Stability: " + currentES);
            //HandleBarDrop();
        }
        
    }

    /*
    private void HandleBarDrop()
    {
        ESbar.transform.position = new Vector2 (barX, barY);
        Debug.Log("Emotional Stability: " + currentES);
    }
    */

    private void GameOver()
    {
        quitTimer += Time.deltaTime;
        if (quitTimer > 3f)
        {
            Application.Quit();
        }
    }
}
