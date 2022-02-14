﻿using System;
using System.Collections;
using Opponents;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Player{
    public class PlayerDebateActionsScript : MonoBehaviour
    {
        [SerializeField] private Text notifyText;
        [SerializeField] private Text damageText;
        [SerializeField] private Transform damageEnd;
        [SerializeField] private DebateHUDScript opponentHUD;
        private DebateSystemScript _debateSystem;
        private GameObject _opponentGO;
        private static OpponentDebateValues _opponentValues;
        private static int _playerDamage;
        private static PlayerDebateValues _playerValues;
        private DebateState _turnState;
        private Vector3 _damagePos;
        private float _damageEndY;

        public bool playerHadTurn;

        private void Start()
        {
            _debateSystem = GetComponentInParent<DebateSystemScript>();
            _opponentGO = GetComponentInParent<DebateSystemScript>().opponentGO;
            if (_opponentGO.GetComponent<DebateValuesScript>())
            {
                _opponentValues = _opponentGO.GetComponent<OpponentDebateValues>();
            }
            else
            {
                Debug.LogError("Supplied Game Object is not an opponent");
            }

            _damageEndY = damageEnd.position.y;
            _damagePos = damageText.transform.position;
            _playerValues = GameObject.FindWithTag("Player").GetComponent<PlayerDebateValues>();
            _playerDamage = _playerValues.debaterLevel*2;
            
        }

        private void CheckPlayerTurn(int emotion){
            if(_debateSystem.state == DebateState.Player){
                OpponentESChange(emotion);
            }
        }
        public void HappyButton()
        {
            CheckPlayerTurn(0);
        }

        public void SadButton()
        {
            CheckPlayerTurn(1);
        }
        
        public void AngryButton()
        {
            CheckPlayerTurn(2);
        }
        
        public void ConfidentButton()
        {
            CheckPlayerTurn(3);
        }

        public void AnxiousButton()
        {
            CheckPlayerTurn(4);
        }

        public void RunButton()
        {
            if(_debateSystem.state == DebateState.Player){
                StartCoroutine(_debateSystem.EndDebate());
            }
        }

        public void WinButton()
        {
            _opponentValues.currentES = 100;
            _debateSystem.PlayerHadTurn = true;
        }
        

        void OpponentESChange(int emotion)
        {
            float emotMult = 0;
            var damageDone = 0;
            var randDamage = Random.Range(-3, 3);
            var overloads = PlayerPrefs.GetInt("Overloads", 0);
            //Debug.Log(overloads);
            var pacifies = PlayerPrefs.GetInt("Pacifies", 0);
            var opponentES = _opponentValues.currentES;
            var opponentEmot = _opponentValues.emotionInt;
            var emotStrengths = GameManager.EmotionStrengths[opponentEmot];
            var emotAmounts = _playerValues.emotAmounts;
            var opponentDifficulty = _opponentValues.emotAmounts[emotion]/10f;
            
            Color emotColor;
            if (emotion == 0)
            {
                emotColor = Color.green;
            }else if (emotion == 1)
            {
                emotColor = Color.blue;
            }else if (emotion == 2)
            {
                emotColor = Color.red;
            }
            else if (emotion == 3)
            {
                emotColor = Color.magenta;
            }
            else
            {
                emotColor = Color.yellow;
            }
            if (emotStrengths[0] == emotion)
            {
                emotMult = 1f;
            }
            if (emotStrengths[1] == emotion)
            {
                emotMult = -2f;
            }
            else if (emotStrengths[2] == emotion)
            {
                emotMult = -1.5f;
            }
            else if (emotStrengths[3] == emotion)
            {
                emotMult = 1.5f;
            }
            else
            {
                emotMult = 2f;
            }
            var modEmot = _playerDamage*emotMult;
            var moddedDamage = (2*modEmot+(modEmot+overloads-pacifies));
            damageDone = Mathf.RoundToInt(moddedDamage/opponentDifficulty)+randDamage;
            opponentES -= damageDone;
            StartCoroutine(DamageGrow(damageDone*-1, emotColor));
            
            var changedBy = "not changed";
            var overOrPass = "unaffected";
            if(damageDone > 0)
            {
                changedBy = $"decreased by {Mathf.Abs(damageDone)}";
                overOrPass = "more passive";
            }else if(damageDone < 0)
            {
                changedBy = $"increased by {Mathf.Abs(damageDone)}";
                overOrPass = "more emotional";
            }
            notifyText.text = $"The {_opponentValues.debaterName}'s emotional strain has {changedBy}.";
            notifyText.text += $" They seem {overOrPass}";

            if (opponentES > _opponentValues.maxES)
            {
                opponentES = _opponentValues.maxES;
            }
            else if (opponentES < -100)
            {
                opponentES = -100;
            }

            //Increases the power of the emotion the player 
            /*if (emotAmounts[emotion] >= 20)
            {
                Mathf.Clamp(emotAmounts[emotion]++,0,20);
            }

            foreach (int emot in emotAmounts)
            {
                if (emot != emotion && emotAmounts[emot] > 0)
                {
                    emotAmounts[emot]--;
                    Mathf.Clamp(emotAmounts[emotion],0,20);
                }
            }*/

            _opponentValues.currentES = opponentES;
            opponentHUD.SetES(_opponentValues);
            _debateSystem.PlayerHadTurn = true;
        }

        IEnumerator DamageGrow(int damageDone, Color emotColour)
        {
            damageText.text = damageDone.ToString();
            if(damageDone > 0)
            {
                damageText.text = $"+{damageText.text}";
            }
            damageText.gameObject.SetActive(true);
            damageText.color = emotColour;
            var damageTextColor = damageText.color;
            while (damageText.transform.position.y < _damageEndY)
            {
                damageText.transform.Translate(0,10,0);
                damageText.fontSize += 4;
                damageTextColor.a -= 2f;
                yield return new WaitForSeconds(0.05f);
            }
            damageText.gameObject.SetActive(false);
            damageTextColor.a = 255;
            damageText.transform.position = _damagePos;
            damageText.fontSize = 30;
            yield return null;
        }
    }
}
