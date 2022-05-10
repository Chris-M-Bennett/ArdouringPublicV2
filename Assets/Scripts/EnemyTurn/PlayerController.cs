using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EnemyTurn;
using Opponents;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject player;
    //private GameObject player;
    //public GameObject text;
    public GameObject eSbar;
    private static DebateValuesScript _playerValues;
    //private static SavePoint _save;
    //private static EnemyController _opponent;
    //private int opponentDamage;
    private float barScale;
    //public GameObject dss;
    //public int debateES;
    // Start is called before the first frame update
    void Start()
    {
        posX = -3.25f;
        posY = -2.06f;
        xStrafe = 0.33f;
        farRightX = posX - 0.01f + 2 * xStrafe;
        farLeftX = posX + 0.01f - 2 * xStrafe;
        player.transform.position = new Vector2 (posX, posY);
        //_playerValues = player.GetComponent<DebateValuesScript>();
        maxES = 100;
        //currentES = 100;
        _playerValues = GameObject.FindWithTag("Player").GetComponent<DebateValuesScript>();
        //_save = GameObject.FindWithTag("Save").GetComponent<SavePoint>();
        //_opponent = GameObject.FindWithTag("EnemyTurn").GetComponent<EnemyController>();
        //opponentDamage = _opponent.bulletDamage;
        /*
        dss = GameObject.FindGameObjectWithTag("Debate");
        if (dss != null)
        {
            //currentES = dss.GetComponent<DebateSystemScript>().debatePlayerES;
        }
        else
        {
            currentES = maxES;
        }
        */
        if (GameManager.healed)
        {
            currentES = maxES;
            _playerValues.currentES = currentES;
            GameManager.healed = false;
        }
        else
        {
            currentES = _playerValues.currentES;
        }
        
        /*
        if (_save.healed)
        {
            currentES = maxES;
        }
        else
        {
            currentES = _playerValues.currentES;
        } */
        
        //barScale = maxES / opponentDamage;
        // Debug.Log("CurrentES: " + currentES);
        esDrop = 0.133f; // * 10) / barScale;
        barX = -2.335f;
        barY = -1.565f - (esDrop * (maxES - currentES)/10); 
        //barY = (-1.565f  * ((maxES - currentES)/100))-1.565f;
        //HandleBarDrop();
        
        
        eSbar.transform.position = new Vector2 (barX, barY);

        // debug messages I used to figure out why the player could move off the left side of the screen instead of stopping.
        //Debug.Log("Right strafe limit: " + farRightX);
        //Debug.Log("Left strafe limit: " + farLeftX);
        //Debug.Log("X strafe amount: " + xStrafe);
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
                //GameObject note = Instantiate(text,new Vector2(2f,0f),Quaternion.identity);
                //Debug.Log("Game Over");
            }
            //GameOver();
        }
    }

    private void HandleStrafe()
    {
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && posX < farRightX)
        {
            posX += xStrafe;
            player.transform.position = new Vector2 (posX, posY);
            //Debug.Log("Player X: " + posX);
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && posX > farLeftX) //this was originally posX != farLeftX but due to floating point arithmetic farLextX is 0.67999999999999 etc instead of 0.68 so the > circumvents this.
        {
            posX -= xStrafe;
            player.transform.position = new Vector2 (posX, posY);
            //Debug.Log("Player X: " + posX);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D hit)
    {
        
        if (hit.gameObject.tag == "Bullet")
        {
            currentES -= hit.GetComponent<BulletController>().damage;
            _playerValues.currentES = currentES;
            //_save.healed = false;
            
            barY -= (esDrop * 10) / (maxES/hit.GetComponent<BulletController>().damage);
            eSbar.transform.position = new Vector2 (barX, barY);
            //Debug.Log("Emotional Stability: " + currentES);
            /*
            if (dss != null)
            {
                //dss.GetComponent<DebateSystemScript>().debatePlayerES = currentES;
            }
            */
            //HandleBarDrop();
            //dss._playerValues.currentES = currentES;
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
            SceneManager.LoadSceneAsync("Overworld");
        }
    }
}
