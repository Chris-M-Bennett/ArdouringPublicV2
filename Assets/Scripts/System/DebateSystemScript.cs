using System.Collections;
using Opponents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
    public enum DebateState {Start, Player, Opponent, Won, Lost}

    public class DebateSystemScript : MonoBehaviour
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

            playerHUD.SetHUD(_playerValues);
            opponentHUD.SetHUD(_opponentValues);
            state = DebateState.Player;
            StartCoroutine(PlayerTurn());
        }

        /// <summary>
        /// Co-routine for the player's turn
        /// </summary>
        /// <returns></returns>
        IEnumerator PlayerTurn()
        {
            //Waits for Boolean to be set by PLayer Actions script
            while (PlayerHadTurn == false)
            {
                yield return null;
            }

            StartCoroutine(DamageAnim(opponentGO));
            _opponentValues.CheckThreshold();
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

        IEnumerator OpponentTurn()
        {
            _playerValues.currentES -= _opponentValues.debaterDamage;
            notifyText.text =
                $"{_opponentValues.debaterName} dealt {_opponentValues.debaterDamage} points of emotional strain to you";
            StartCoroutine(DamageAnim(player));
            playerHUD.SetES(_playerValues.currentES);
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

            yield return null;
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

        IEnumerator DamageAnim(GameObject debaterGO)
        {
            int i = 0;
            Vector2 dir = new Vector2(0.1f, 0);
            while (i < 6)
            {
                //Debug.Log(dir);
                debaterGO.transform.Translate(dir);
                dir *= -1;
                i++;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}