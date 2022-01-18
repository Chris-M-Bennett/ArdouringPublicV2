using System.Collections;
using System.Collections.Generic;
using Opponents;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

namespace System
{
    public enum DebateState {Start, Player, Opponent, Won, Lost}

    public class DebateSystemScript : MonoBehaviour
    {
        public List<Vector2> Directions = new List<Vector2>();
        public List<float> Values = new List<float>();

        public Vector2 Result = new Vector2(0, 0);
        
        [SerializeField, Header("The Player game object in the scene")]
        private GameObject player;
        [SerializeField, Header("The point where the enemy is placed")]
        private Transform opponentSpawn;
        [SerializeField] private GameObject testPrefab;
        [SerializeField, Header("The text box for information")] 
        private UnityEngine.UI.Text notifyText;
        [SerializeField] private Image background;

        [SerializeField] private MoveBarsScript tranBars;
        [SerializeField] private EventSystem eventSystem;

        [Header("The player's HUD panel")]public DebateHUDScript playerHUD;
        [Header("The opponent's HUD panel")]public DebateHUDScript opponentHUD;
        
        public GameObject enemyTurn;
        public float trackX = -4.25f, trackY = -2.48f;
        
        private bool _playerHadTurn = false;
        private DebateValuesScript _playerValues;
        private DebateValuesScript _opponentValues;
        private int _playerExp;
        private int _playerLevel;
        private int _opponentPrevES;
        public bool PlayerHadTurn
        {
            get => _playerHadTurn;
            set => _playerHadTurn = value;
        }

        [HideInInspector] public GameObject opponentGO;
        [HideInInspector]public DebateState state = DebateState.Start;
        private IEnumerator _debateSetup;

        void Start()
        {
            background.sprite = GameManager.debateBG;
            _playerValues = player.GetComponent<DebateValuesScript>();
            if (GameManager.debateOpponent)
            {
                opponentGO = Instantiate(GameManager.debateOpponent, opponentSpawn);
            }else
            {
                opponentGO = Instantiate(testPrefab, opponentSpawn);   
            }

            _opponentValues = opponentGO.GetComponent<DebateValuesScript>();
            _opponentPrevES = _opponentValues.currentES;

            playerHUD.SetHUD(_playerValues);
            _playerExp = PlayerPrefs.GetInt("playerEX", 0);
            opponentHUD.SetHUD(_opponentValues);
            state = DebateState.Player;
            StartCoroutine(tranBars.MoveThoseBars(false));
            StartCoroutine(PlayerTurn());
        }

        /// <summary>
        /// Co-routine for player's turn
        /// </summary>
        /// <returns>5 second wait before opponent's turn</returns>
        IEnumerator PlayerTurn()
        {
            if (GameManager.tutorials)
            {
                int opponentEmot = _opponentValues.emotionInt;
                notifyText.text =
                    $"It's your turn! Selected an emotion button from your panel to debate with the creature.\n";
                notifyText.text += EmotionDescript(opponentEmot);
            }
            
            //Waits for Boolean to be set by PLayer Actions script
            while (PlayerHadTurn == false)
            {
                yield return null;
            }

            StartCoroutine(DamageAnim(opponentGO));
            
            if (_opponentValues.currentES <= -100 || _opponentValues.currentES >= 100)
            {
                if (_opponentValues.currentES <= -100)
                {
                    PlayerPrefs.SetInt("Pacifies",PlayerPrefs.GetInt("Pacifies",0)+1);
                }
                else
                {
                    PlayerPrefs.SetInt("Overloads",PlayerPrefs.GetInt("Overloads",0)+1);
                }
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
            
            GameObject foe = Instantiate(enemyTurn,new Vector2(trackX,trackY),Quaternion.identity);
            _opponentValues.CheckThreshold(_opponentPrevES);
            notifyText.text +=
                $" {_opponentValues.debaterName} dealt {_opponentValues.debaterDamage} points of emotional strain to you";
            eventSystem.enabled = false;
            StartCoroutine(DamageAnim(player));
            playerHUD.SetES(_playerValues);
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
            
            _opponentPrevES = _opponentValues.currentES;
            yield return new WaitForSeconds(1f);
        }
        
        /// <summary>
        /// Co-routine for end of the debate
        /// </summary>
        /// <returns>2 second wait before loading Overworld</returns>

        public IEnumerator EndDebate()
        {
            if (_playerExp == 4)
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
                GameManager.wonDebate = true;
            }
            else if (state == DebateState.Lost)
            {
                _playerValues.currentES = _playerValues.maxES;
                PlayerPrefs.SetInt("playerES", _playerValues.maxES);
                _playerExp += 1;
                notifyText.text = "You lost the debate!";
            }
            else
            {
                notifyText.text = "You fled the fight.";
            }
            
            PlayerPrefs.SetInt("playerExp", _playerExp);
            yield return new WaitForSeconds(2f);
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
            Result = Vector3.zero;
            for (int i = 0; i < Directions.Count; i++)
            {
                var v = Directions[i] * Values[i];
                Result += v;
            }
            Debug.DrawLine(Vector3.zero,Result,Color.red,0.1f);
        }

        /// <summary>
        /// Generates tutorial for player about opponent emotions and how other emotions effect them 
        /// </summary>
        /// <param name="emotInt"></param>
        /// <returns></returns>
        private string EmotionDescript(int emotInt)
        {
            string colour;
            string emot;
            string very;
            string fairly;
            string barely;
            if (emotInt == 0)
            {
                colour = "Green";
                emot = "happy";
                very = "sad";
                fairly = "angry";
                barely = "afraid";
            }else if (emotInt == 1)
            {
                colour = "Blue";
                emot = "sad";
                very = "angry";
                fairly = "confident";
                barely = "afraid";
            }else if (emotInt == 2)
            {
                colour = "Red";
                emot = "angry";
                very = "confident";
                fairly = "afraid";
                barely = "happy";
            }else if (emotInt == 3)
            {
                colour = "Purple";
                emot = "confident";
                very = "afraid";
                fairly = "happy";
                barely = "sad";
            }else if (emotInt == 4)
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