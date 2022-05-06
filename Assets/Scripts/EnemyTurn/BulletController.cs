using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using EnemyTurn;
using Opponents;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private BulletController _bullet;
    public int damage;
    private static DebateValuesScript _opponentValues;
    private static EnemyController _enemy;
    private static PlayerController _player;
    private float speed;
    private float distance;
    private float targetY;
    /**/
    public string[,] bulletLibrary = new string[17, 3]
    {
        {"Chameleon","1.2","12"}, //used to be Calmer Chameleon, now only Chameleon works, idk why
        {"Chice","1.5","7"},
        {"Storming Cloud","1.5","10"},
        {"Inferno","2.1","10"},
        {"Laughing Cat","1","10"},
        {"Sadgull","0.8","12"},
        {"Space Whale","0.9","10"},
        {"The Upset Post","1","10"},
        {"Tutorial Goblin","1","1"},
        {"Tall Bird","1.3","12"},
        {"Aire","0.8","8"},
        {"Nacho Rodriguez","1.1","8"},
        {"Heartyfish","1.4","9"},
        {"Meppo","0.7","13"},
        {"Foffalo","1","8"},
        {"Blueser","1.4","10"},
        {"Null","1.7","10"}
    };
    //*/
    // Start is called before the first frame update
    void Start()
    {
        _opponentValues = GameObject.FindWithTag("Opponent").GetComponent<DebateValuesScript>();
        /*
        _enemy = GameObject.FindWithTag("EnemyTurn").GetComponent<EnemyController>();
        damage = _enemy.bulletDamage;
        speed = _enemy.bulletSpeed;
        */
        /**/
        bool matchedDebater = false;
        for (int b = 0; b < bulletLibrary.GetLength(0); b++)
        {
            if (bulletLibrary[b, 0] == _opponentValues.debaterName)
            {
                speed = float.Parse(bulletLibrary[b, 1], CultureInfo.InvariantCulture.NumberFormat);
                damage = int.Parse(bulletLibrary[b, 2], CultureInfo.InvariantCulture.NumberFormat);
                matchedDebater = true;
            }
        }
        if (!matchedDebater)
        {
            speed = 1f;
            damage = 10;
        }
        //*/
        //damage = 10;
        
        targetY = -1.8f;//-4.2f;//-1.8f; //1.32f; //0.32f; used to stop off screen, now stop immediately before player (still collide but don't stick around after being dodged)
        _bullet = GetComponent<BulletController>();
        _player = GetComponent<PlayerController>();
        //speed = 1f;//_opponent.bulletSpeed;
        //Debug.Log("bullet speed: " + speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float currentY = _bullet.transform.position.y;

        distance = currentY - targetY;

        if (distance > 0f)
        {
            float direction = -1f; //targetY - currentY;
            _bullet.transform.Translate(0f, direction * speed * Time.deltaTime, 0f, Space.World);
            if (_opponentValues.debaterName == "Space Whale")
            {
                Vector2 dir = (_bullet.transform.position - _player.transform.position).normalized;
                _bullet.transform.Translate(dir.x, 0f * 0.3f * Time.deltaTime, 0f, Space.World);
            }

            if (_opponentValues.debaterName == "Laughing Cat")
            {
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D end)
    {

        if (end.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
   
