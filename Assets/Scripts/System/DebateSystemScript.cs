using System.Collections;
using Opponents;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
    public enum DebateState {Start, Player, Opponent, Won, Lost}

    public partial class DebateSystemScript : MonoBehaviour
    {
        [SerializeField, Header("The Player game object in the scene")]
        private GameObject player;
        [SerializeField, Header("The point where the enemy is placed")]
        private Transform opponentSpawn;
        [SerializeField, Header("The Opponent Prefab to spawn")] 
        private GameObject opponentPrefab;
        [SerializeField, Header("The text box for information")] 
        private UnityEngine.UI.Text notifyText;

        [Header("The player's HUD panel")]public DebateHUDScript playerHUD;
        [Header("The opponent's HUD panel")]public DebateHUDScript opponentHUD;
        
        private bool _playerHadTurn = false;
        private DebateValuesScript _playerValues;
        private DebateValuesScript _opponentValues;
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
            _playerValues = player.GetComponent<DebateValuesScript>();

            opponentGO = Instantiate(opponentPrefab, opponentSpawn);
            _opponentValues = opponentGO.GetComponent<DebateValuesScript>();
            _opponentPrevES = _opponentValues.currentES;

            playerHUD.SetHUD(_playerValues);
            opponentHUD.SetHUD(_opponentValues);
            state = DebateState.Player;
            StartCoroutine(PlayerTurn());
        }

        /// <summary>
        /// Co-routine for player's turn
        /// </summary>
        /// <returns>5 second wait before opponent's turn</returns>
        IEnumerator PlayerTurn()
        {
            if (GameManager.Tutorials)
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
            if (_opponentValues.currentES <= 0)
            {
                state = DebateState.Won;
                StartCoroutine(EndDebate());
            }
            else
            {
                state = DebateState.Opponent;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(OpponentTurn());
            }
        }

        /// <summary>
        /// Co-routine for opponent's turn
        /// </summary>
        /// <returns></returns>
        IEnumerator OpponentTurn()
        {
            _opponentValues.CheckThreshold(_opponentPrevES);
            _playerValues.currentES -= _opponentValues.debaterDamage;
            notifyText.text +=
                $" {_opponentValues.debaterName} dealt {_opponentValues.debaterDamage} points of emotional strain to you";
            StartCoroutine(DamageAnim(player));
            playerHUD.SetES(_playerValues);
            if (_playerValues.currentES <= 0)
            {
                state = DebateState.Lost;
                StartCoroutine(EndDebate());
            }
            else
            {
                state = DebateState.Player;
                PlayerHadTurn = false;
                StartCoroutine(PlayerTurn());
            }
            
            _opponentPrevES = _opponentValues.currentES;
            yield return new WaitForSeconds(1f);
        }

        public IEnumerator EndDebate()
        {
            if (_playerValues.playerExp == 4)
            {
                _playerValues.debaterLevel += 1;
                _playerValues.playerExp = 0;
            }
            if (state == DebateState.Won)
            {
                _playerValues.playerExp += 2;
                notifyText.text = "You won the debate!";
            }
            else if (state == DebateState.Lost)
            {
                _playerValues.currentES = _playerValues.maxES;
                PlayerPrefs.SetInt("playerES", _playerValues.maxES);
                _playerValues.playerExp += 1;
                notifyText.text = "You lost the debate!";
            }
            else
            {
                notifyText.text = "You fled the fight.";
            }
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


        private string EmotionDescript(int emotInt)
        {
            string colour;
            string emot;
            string very;
            string fairly;
            string barely;
            Debug.Log(emotInt);
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
                fairly = "afraid";
                barely = "happy";
            }else if (emotInt == 2)
            {
                colour = "Red";
                emot = "angry";
                very = "afraid";
                fairly = "happy";
                barely = "sad";
            }else if (emotInt == 3)
            {
                colour = "Red";
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