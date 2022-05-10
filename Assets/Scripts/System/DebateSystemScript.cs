using System.Collections;
using System.Collections.Generic;
using System.IO;
using Opponents;
using Player;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

namespace System
{
    public enum DebateState {Start, Player, Opponent, End, Won, Lost}

    public class DebateSystemScript : MonoBehaviour
    {
        public List<Vector2> directions = new List<Vector2>();
        public List<float> values = new List<float>();

        public Vector2 result = new Vector2(0, 0);
        
        private float timeTaken;
        
        [SerializeField, Header("The Player game object in the scene")]
        private GameObject player;
        [SerializeField, Header("The point where the enemy is placed")]
        private Transform opponentSpawn;
        [SerializeField] private GameObject testPrefab;
        [SerializeField, Header("The text box for information")] 
        private UnityEngine.UI.Text notifyText;
        [SerializeField, Header("The text box for player statistics")] 
        private UnityEngine.UI.Text statsText;
        [SerializeField] private Image background;

        [SerializeField] private MoveBarsScript tranBars;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject statsBooster;
        [SerializeField] private LastOpponentTracker tracker;

        [Header("The player's HUD panel")]public DebateHUDScript playerHud;
        [Header("The opponent's HUD panel")]public DebateHUDScript opponentHud;
        
        public GameObject enemyTurn;
        public float trackX = -4.25f, trackY = -2.48f;
        //public string file;
        
        private bool _playerHadTurn = false;
        private DebateValuesScript _playerValues;
        private OpponentDebateValues _opponentValues;
        private int _playerExp;
        private int _playerLevel;
        private int _playerEmot;
        private int _opponentStatus = 0;
        private GameObject _speechBubble;

        [HideInInspector] public bool confirmExit = false; 
        
        public bool PlayerHadTurn
        {
            get { return _playerHadTurn; }
            set { _playerHadTurn = value; }
        }

        public int PlayerEmot
        {
            get { return _playerEmot; }
            set { _playerEmot = value;}
        }

        [HideInInspector] public GameObject opponentGO;
        [HideInInspector]public DebateState state = DebateState.Start;
        private IEnumerator _debateSetup;

        private void Awake()
        {
            if (!(Camera.main is null))
            {
                var main = Camera.main;
                var pixelHeight = main.pixelHeight;
                opponentSpawn.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(
                    main.pixelWidth/6, pixelHeight-(pixelHeight/6), 1));
            }

            if (GameManager.debateOpponent)
            {
                opponentGO = Instantiate(GameManager.debateOpponent, opponentSpawn);
            }else
            {
                opponentGO = Instantiate(testPrefab, opponentSpawn);   
            }

            _opponentValues = opponentGO.GetComponent<OpponentDebateValues>();
        }

        void Start()
        {
            Debug.Log(GameManager.wasBoss);
            background.sprite = GameManager.debateBg;
            _playerValues = player.GetComponent<PlayerDebateValues>();
            _playerValues.currentES = PlayerPrefs.GetInt("playerES", 100);
            playerHud.SetHud(_playerValues);
            _playerExp = PlayerPrefs.GetInt("playerExp", 0);

            opponentHud.SetHud(_opponentValues);
            statsText.text += $"\n\nOverloads: {PlayerPrefs.GetInt("overloads",0)}" +
                             $"\n\nPacifies: {PlayerPrefs.GetInt("pacifies", 0)}";
            
            state = DebateState.Player;

            var statTexts = statsBooster.GetComponent<EmotionStatsChangeScript>();
            statTexts.happyStat.text = PlayerPrefs.GetInt("playerHappy").ToString();
            statTexts.sadStat.text = PlayerPrefs.GetInt("playerSad").ToString();
            statTexts.angryStat.text = PlayerPrefs.GetInt("playerAngry").ToString();
            statTexts.proudStat.text = PlayerPrefs.GetInt("playerProud").ToString();
            statTexts.afraidStat.text = PlayerPrefs.GetInt("playerAfraid").ToString();

            _speechBubble = opponentGO.transform.GetChild(0).gameObject;
            StartCoroutine(_opponentValues.Speak(Stages.Opening));
            StartCoroutine(tranBars.MoveThoseBars(false));
            StartCoroutine(PlayerTurn());
        }
        
        private void Update()
        {
            timeTaken += Time.deltaTime;
            /*    result = Vector3.zero;
                for (int i = 0; i < directions.Count; i++)
                {
                    var v = directions[i] * values[i];
                    result += v;
                }
                Debug.DrawLine(Vector3.zero,result,Color.red,0.1f);*/
        }


        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Co-routine for player's turn
        /// </summary>
        /// <returns>5 second wait before opponent's turn</returns>
        IEnumerator PlayerTurn()
        {
            StopCoroutine(OpponentTurn());

            if (GameManager.tutorials)
            {
                Emotions opponentEmot = _opponentValues.emotionEnum;
                notifyText.text =
                    "It's your turn! Select an emotion button from your panel to debate with the creature.\n";
                notifyText.text += EmotionDescript(opponentEmot);
            }
            
            _opponentValues.prevES = _opponentValues.currentES;

            //Waits for Boolean to be set by Player Actions script
            while (PlayerHadTurn == false)
            {
                yield return null;
            }

            StartCoroutine(DamageAnim(opponentGO));
            
            if (_opponentValues.currentES <= -100 || _opponentValues.currentES >= 100)
            {
                StopCoroutine(_opponentValues.Speak(Stages.Pacified));
                var minutesSeconds = $"{Mathf.FloorToInt(timeTaken/60)}:{Mathf.Clamp(Mathf.RoundToInt(timeTaken%60), 0f, 59f)}";

                _speechBubble.SetActive(true);
                if (_opponentValues.currentES <= -100)
                {
                    StartCoroutine(_opponentValues.Speak(Stages.Pacified));
                    PlayerPrefs.SetInt("Pacifies",PlayerPrefs.GetInt("Pacifies",0)+1);
                    statsBooster.SetActive(true);
                    
                    _opponentStatus = 1;
                }
                else
                {
                    StartCoroutine(_opponentValues.Speak(Stages.Overloaded));
                    PlayerPrefs.SetInt("Overloads",PlayerPrefs.GetInt("Overloads",0)+1);
                    var emotString = "";
                    switch (_playerEmot)
                    {
                        case 0:
                            emotString = "playerHappy";
                            break;
                        case 1:
                            emotString = "playerSad";
                            break;
                        case 2:
                            emotString = "playerAngry";
                            break;
                        case 3:
                            emotString = "playerProud";
                            break;
                        case 4:
                            emotString = "playerAfraid";
                            break;
                        default:
                            Debug.LogError("Invalid player emotion");
                            yield return null;
                            break;
                    }
                    PlayerPrefs.SetInt(emotString,PlayerPrefs.GetInt(emotString)+1);
                    
                    _opponentStatus = -1;
                }
                GameManager.areaStatuses.statuses[tracker.LastOpponent] = _opponentStatus;
                state = DebateState.Won;
                StartCoroutine(EndDebate());
            }
            else
            {
                state = DebateState.Opponent;
                //yield return new WaitForSeconds(2f);
                StartCoroutine(OpponentTurn());
            }
        }

        /// <summary>
        /// Co-routine for opponent's turn
        /// </summary>
        /// <returns></returns>
        IEnumerator OpponentTurn()
        {
            StopCoroutine(PlayerTurn());
            yield return new WaitForSeconds(2f);
            _speechBubble.SetActive(false);
            _opponentValues.CheckThreshold(_opponentValues.currentES);
            
            GameObject foe = Instantiate(enemyTurn,new Vector2(trackX,trackY),Quaternion.identity);
            eventSystem.enabled = false;
            StartCoroutine(DamageAnim(player));
            //playerHud.SetES(_playerValues);
            yield return new WaitForSeconds(12f);
            if (_playerValues.currentES <= 0)
            {
                state = DebateState.Lost;
                StartCoroutine(EndDebate());
            }
            else
            {
                state = DebateState.Player;
                PlayerHadTurn = false;
                Destroy(foe);
                eventSystem.enabled = true;
                StartCoroutine(PlayerTurn());
            }
            yield return new WaitForSeconds(1f);
        }
        
        /// <summary>
        /// Co-routine for end of the debate
        /// </summary>
        /// <returns>2 second wait before loading Overworld</returns>
        public IEnumerator EndDebate()
        {
            StopCoroutine(OpponentTurn());
            if (_playerExp == 10)
            {
                _playerLevel += 1;
                PlayerPrefs.SetInt("playerLevel", _playerLevel);
                
                _playerExp = 0;
            }
            if (state == DebateState.Won)
            {
                _playerExp += 2;
                notifyText.text = "You won the debate!";
            }
            else if (state == DebateState.Lost)
            {
                SceneManager.LoadScene("Game Over");
                StopCoroutine(EndDebate());

            }
            else
            {
                notifyText.text = "You fled the fight.";
            }
            
            PlayerPrefs.SetInt("playerES", _playerValues.currentES);
            PlayerPrefs.SetInt("playerExp", _playerExp);
            
            if (_opponentStatus != 1)
            {
                confirmExit = true;
                yield return new WaitForSeconds(4f);
            }
            
            while (!confirmExit)
            {
                yield return null;
            }

            if (confirmExit)
            {
                SceneManager.LoadSceneAsync(GameManager.overworld);
            }
        }

        /// <summary>
        /// Co-routine to make damaged debater appear to stagger
        /// </summary>
        /// <param name="debaterGO"></param>
        /// <returns>100 millisecond wait before moving position</returns>
        IEnumerator DamageAnim(GameObject debaterGO)
        {
            int i = 0;
            Vector2 dir = new Vector2(0.1f, 0);
            while (i < 6)
            {
                debaterGO.transform.Translate(dir);
                dir *= -1;
                i++;
                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Generates tutorial for player about opponent emotions and how other emotions effect them 
        /// </summary>
        /// <param name="emotEnum"></param>
        /// <returns></returns>
        private string EmotionDescript(Emotions emotEnum)
        {
            string emot;
            string strongOveload;
            string weakOverload;
            string weakPacify;
            string strongPacify;
            if (emotEnum == Emotions.Happy)
            {
                emot = "happy";
                strongOveload = "sad";
                weakOverload = "angry";
                strongPacify = "proud";
                weakPacify = "afraid";
            }else if (emotEnum == Emotions.Sad)
            {
                emot = "sad";
                strongOveload = "angry";
                weakOverload = "proud";
                strongPacify = "afraid";
                weakPacify = "happy";
            }else if (emotEnum == Emotions.Angry)
            {
                emot = "angry";
                strongOveload = "proud";
                weakOverload = "afraid";
                strongPacify = "happy";
                weakPacify = "sad";
            }else if (emotEnum == Emotions.Proud)
            {
                emot = "proud";
                strongOveload = "afraid";
                weakOverload = "happy";
                strongPacify = "sad";
                weakPacify = "angry";
            }else if (emotEnum == Emotions.Afraid)
            {
                emot = "afraid";
                strongOveload = "happy";
                weakOverload = "sad";
                strongPacify = "angry";
                weakPacify = "proud";
            }else
            {
                Debug.LogError("Invalid emotion supplied to EmotionDescript function!");
                return null;
            }
            return $"This opponent is {emot} and {strongOveload} actions have a greater impact towards overloading, " +
                   $"{weakOverload} actions have a lesser impact towards overloading, {strongPacify} actions have a " +
                   $" greater impact towards pacifying and {weakPacify} actions have a lesser impact towards pacifying. " +
                   $"Using the {emot} action has a very slight varying effect that can impact towards overloading or pacifying.";
        }
    }
}