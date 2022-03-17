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

        [HideInInspector] public bool confirmExit = false; 
        [HideInInspector] public bool PlayerHadTurn
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

        void Start()
        {
            background.sprite = GameManager.debateBg;
            _playerValues = player.GetComponent<PlayerDebateValues>();
            _playerValues.currentES = PlayerPrefs.GetInt("playerES", 100);
            playerHud.SetES(_playerValues);
            _playerExp = PlayerPrefs.GetInt("playerExp", 0);

            if (GameManager.debateOpponent)
            {
                opponentGO = Instantiate(GameManager.debateOpponent, opponentSpawn);
            }else
            {
                opponentGO = Instantiate(testPrefab, opponentSpawn);   
            }

            _opponentValues = opponentGO.GetComponent<OpponentDebateValues>();
            
            opponentHud.SetHud(_opponentValues);
            statsText.text = $"Happy Power: {PlayerPrefs.GetInt("playerHappy", 1)}" +
                             $"\n\nSad Power: {PlayerPrefs.GetInt("playerSad", 1)}" +
                             $"\n\nAngry Power: {PlayerPrefs.GetInt("playerAngry", 1)}" +
                             $"\n\nConfident Power: {PlayerPrefs.GetInt("playerConfident", 1)}" +
                             $"\n\nAfraid Power: {PlayerPrefs.GetInt("playerAfraid", 1)}" +
                             $"\n\nOverloads: {PlayerPrefs.GetInt("overloads",0)}" +
                             $"\n\nPacifies: {PlayerPrefs.GetInt("pacifies", 0)}";
            
            state = DebateState.Player;
            
            /*var address = "Assets/Dialogue/Dialogue.csv";
            if(File.Exists(getPath(address),FileMode.Open, FileAccess.ReadWrite))
            {'
                StreamReader dialStream = new StreamReader("(Assets/Dialogue/Dialogue.csv)");
                file = dialStream.ReadToEnd(); 
            } else
            {
                Debug.LogError($"File at {address} does not exist");
            }*/
            
            _opponentValues.Speak(Stages.Opening);

            StartCoroutine(tranBars.MoveThoseBars(false));
            StartCoroutine(PlayerTurn());
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
                if (_opponentValues.currentES <= -100)
                {
                    _opponentValues.Speak(Stages.Pacified);
                    PlayerPrefs.SetInt("Pacifies",PlayerPrefs.GetInt("Pacifies",0)+1);
                    
                    _opponentStatus = 1;
                }
                else
                {
                    _opponentValues.Speak(Stages.Overloaded);
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
                            emotString = "playerConfident";
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
                GameManager.areaStatuses.statuses[LastOpponent.lastOpponent] = _opponentStatus;
                state = DebateState.Won;
                StartCoroutine(EndDebate());
            }
            else
            {
                state = DebateState.Opponent;
                yield return new WaitForSeconds(2f);
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
            _opponentValues.CheckThreshold(_opponentValues.currentES);
            GameObject foe = Instantiate(enemyTurn,new Vector2(trackX,trackY),Quaternion.identity);
            //notifyText.text +=
            //    $" {_opponentValues.debaterName} dealt {_opponentValues.debaterDamage} points of emotional strain to you";
            eventSystem.enabled = false;
            StartCoroutine(DamageAnim(player));
            playerHud.SetES(_playerValues);
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
                //yield return new WaitForSeconds(1f);
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

                /*foreach (int emot in _playerValues.emotAmounts)
                {
                    _playerValues.emotAmounts[emot]++;
                }*/
                _playerExp = 0;
            }
            if (state == DebateState.Won)
            {
                _playerExp += 2;
                notifyText.text = "You won the debate!";
                if (_opponentStatus != 1)
                {
                    confirmExit = true;
                }
            }
            else if (state == DebateState.Lost)
            {
                // _playerExp += 1;
                notifyText.text = "You lost the debate!";
            }
            else
            {
                notifyText.text = "You fled the fight.";
            }
            
            PlayerPrefs.SetInt("playerES", _playerValues.currentES);
            PlayerPrefs.SetInt("playerExp", _playerExp);
            yield return new WaitForSeconds(2f);
            /*while (confirmExit == false)
            {
                yield return null;
            }*/
            SceneManager.LoadSceneAsync("Overworld");
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

        private void Update()
        {
            result = Vector3.zero;
            for (int i = 0; i < directions.Count; i++)
            {
                var v = directions[i] * values[i];
                result += v;
            }
            Debug.DrawLine(Vector3.zero,result,Color.red,0.1f);
        }

        private void ChangeStats(string emotion)
        {
            
        }

        /// <summary>
        /// Generates tutorial for player about opponent emotions and how other emotions effect them 
        /// </summary>
        /// <param name="emotEnum"></param>
        /// <returns></returns>
        private string EmotionDescript(Emotions emotEnum)
        {
            string colour;
            string emot;
            string very;
            string fairly;
            string barely;
            if (emotEnum == Emotions.Happy)
            {
                colour = "Green";
                emot = "happy";
                very = "sad";
                fairly = "angry";
                barely = "afraid";
            }else if (emotEnum == Emotions.Sad)
            {
                colour = "Blue";
                emot = "sad";
                very = "angry";
                fairly = "confident";
                barely = "afraid";
            }else if (emotEnum == Emotions.Angry)
            {
                colour = "Red";
                emot = "angry";
                very = "confident";
                fairly = "afraid";
                barely = "happy";
            }else if (emotEnum == Emotions.Proud)
            {
                colour = "Purple";
                emot = "confident";
                very = "afraid";
                fairly = "happy";
                barely = "sad";
            }else if (emotEnum == Emotions.Afraid)
            {
                colour = "Orange";
                emot = "afraid";
                very = "happy";
                fairly = "sad";
                barely = "angry"; 
            }else
            {
                Debug.LogError("Invalid emotion supplied to EmotionDescript function!");
                return null;
            }
            return $"{colour} opponents are {emot} and are very weak to {very} actions," +
                   $" fairly weak to {fairly} actions, and barely weak to {barely} actions.";
        }
    }
}