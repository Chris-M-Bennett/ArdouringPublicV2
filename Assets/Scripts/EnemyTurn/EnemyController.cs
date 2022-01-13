using System;
using System.Globalization;
using Opponents;
using UnityEngine;
using Random = System.Random;

namespace EnemyTurn{
    public class EnemyController : MonoBehaviour
    {
        //public EnemyController _bullet;
        public GameObject bullet;
        private float bulletInterval;
        private float timer;
        public float turnTimer;
        public float turnLimit;
        //private float bulletSize = 0.36f;
        public float startY;
        public float bulletX;
        public GameObject enemyTurn;
        public char currentEmotion;
        public int maxEnemyES;
        public string nickname;
        private static DebateValuesScript _opponentValues;
        //public Vector2 startPos;
        /*
    public bool[,] attackPattern = new bool[4,5]
    {
        {true, false, true, true, false},
        {false, false, true, false, true},
        {true, true, false, false, true},
        {false, true, false, true, false}
    }; */// an array of sets of x coordiantes
        private string patternString;
        private int patternLength;
        //public char[,] attackPattern;
    
        public char[,] attackPattern;
        /*
    {
        {'~', '.', '~', '~', '.'},
        {'.', '.', '~', '.', '~'},
        {'~', '~', '.', '.', '~'},
        {'.', '~', '.', '~', '.'}
    };// future-proofing if emotionally charged bullets are added later (s = sad, a = angry, h = happy, f = afraid) ~ = bullet, . = no bullet
    */
        private int patternRow;
        private int rows;
        public string[,] patternLibrary = new string[2,3]
        {
            {"Calmer Chameleon","~~..~~~......~~..~~~......~..~~..~...~.~......~.~.~.~...~..~.....~.....~.....~.....~..........~...~...~...~........~....","0.33"},
            {"Chice","~.~.~.~.~.~.~.~.~.~.~.~.~","0.3"}
        };
    
        /*
    public EnemyController(char[,] attackPattern)
    {
        this.attackPattern = attackPattern;
    }
*/

        // Start is called before the first frame update
        void Start()
        {
            //bullet starting y = 5.28f, ending y = 0.32f
            startY = 0.16f; //5.28f;
            bulletX = -3.25f; //2f;
            timer = 0f;
            turnTimer = 0f;
            turnLimit = 10f;
            //bulletInterval = 0.33f; //0.66f;
            patternRow = 0;
            //patternString = "~~..~~~......~~..~~~......~..~~..~...~.~......~.~.~.~...~..~.....~.....~.....~.....~..........~...~...~...~........~....";
            _opponentValues = GameObject.FindWithTag("Opponent").GetComponent<DebateValuesScript>();
            Debug.Log("Opponent: " + _opponentValues.debaterName);
            bool matchedDebater = false;
            for (int m = 0; m < patternLibrary.GetLength(0); m++)
            {
                if (patternLibrary[m, 0] == _opponentValues.debaterName)
                {
                    patternString = patternLibrary[m, 1];
                    bulletInterval = float.Parse(patternLibrary[m, 2], CultureInfo.InvariantCulture.NumberFormat);
                    matchedDebater = true;
                }

            }
            if (!matchedDebater)
            {
                patternString = "~.~~.......~~.~.....";
                bulletInterval = 0.33f;
            }
            patternLength = patternString.Length;
            rows = (int)Math.Floor((decimal) (patternLength/5));
            attackPattern = new char[rows, 5];
            Debug.Log("Pattern String Length: " + patternLength);
            Debug.Log("Number of Rows: " + rows);
            //for(int p = 0; p < patternLength; p++)
            //{
            int p = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    attackPattern[r, c] = patternString[p];
                    //Debug.Log("row: " + r + ", column: " + c + ", point in pattern string: " + p + ", bullet: " + patternString[p]);
                    p++;
                }
            }
            //}
        }

        // Update is called once per frame
        void Update()
        {
            if (turnTimer < turnLimit)
            {
                //for (int i = 0; i < attackPattern.GetLength(0); i++)
                //{
            
                timer += Time.deltaTime;
                if (timer > bulletInterval)
                {
                    FirePattern(patternRow);
                    //Debug.Log("Attack Pattern #: " + patternRow);
                    if (patternRow < attackPattern.GetLength(0) - 1)
                    {
                        patternRow++;
                    }
                    else
                    {
                        patternRow = 0;
                    }
                    timer = 0f;
                }
                //}
            }
            else
            {
                //Destroy(enemyTurn);
            }
            turnTimer += Time.deltaTime;
        }

        private void RndFirePattern()
        {
            Random rnd = new Random();
            int track = rnd.Next(1, 6);
            Debug.Log("Track number:" + track);
            switch (track)
            {
                case 1:
                    bulletX = 0.68f;
                    break;
                case 2:
                    bulletX = 1.34f;
                    break;
                case 3:
                    bulletX = 2f;
                    break;
                case 4:
                    bulletX = 2.66f;
                    break;
                default: bulletX = 3.32f;
                    break;
            }
            GameObject note = Instantiate(bullet,new Vector2(bulletX,startY),Quaternion.identity);
        }

        private void FirePattern(int i)
        {
            //Random rnd = new Random();
            //int i = rnd.Next(1, 5)-1;
        
            
            //Debug.Log("Timer: " + timer);
            
            for (int j = attackPattern.GetLength(1) -1; j > -1; j--)
            {
                if (attackPattern[i, j] == '~')
                {
                    bulletX = ((j + 1) * 0.33f) + 0.01f - 4.25f;
                    GameObject note = Instantiate(bullet,new Vector2(bulletX,startY),Quaternion.identity);
                }
            }
                
            //Bullet pattern incrementer was looping through rows of the array multiple times before the bullet cooldown
            //ticked up so the order of bullet patterns was unintentionally random. Now the order only increments when
            //the cooldown finishes & calls FirePattern at the same time (the timer has been moved out of that method).
        }
    }
}