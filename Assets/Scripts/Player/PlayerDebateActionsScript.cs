using System;
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
        private static DebateValuesScript _opponentValues;
        private static int _playerDamage;
        private DebateState _turnState;
        private Vector3 _damagePos;
        private float _damageEndY;
        [SerializeField] private int strongMult = 2;

        public bool playerHadTurn;

        private void Start()
        {
            _debateSystem = GetComponentInParent<DebateSystemScript>();
            _opponentGO = GetComponentInParent<DebateSystemScript>().opponentGO;
            if (_opponentGO.GetComponent<DebateValuesScript>())
            {
                _opponentValues = _opponentGO.GetComponent<DebateValuesScript>();
            }
            else
            {
                Debug.LogError("Supplied Game Object is not an opponent");
            }

            _damageEndY = damageEnd.position.y;
            _damagePos = damageText.transform.position;
            _playerDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<DebateValuesScript>().debaterDamage;
            
        }


        /*void SetButtonUnlock(bool state)
    {
        _choice4Button.enabled = state;
        choice4.GetComponent<Image>().color = state ? Color.white : Color.gray;
    }*/

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

        /*void ChangeTexts(string text1, string text2, string text3, string text4){
       //Removes listeners previous added to buttons
        _choice1Button.onClick.RemoveAllListeners();
        _choice2Button.onClick.RemoveAllListeners();
        _choice3Button.onClick.RemoveAllListeners();
        _choice4Button.onClick.RemoveAllListeners();
        
        //Checks text for buttons to see if they should be active
        choice1.SetActive(text1 != "");
        choice2.SetActive(text2 != "");
        choice3.SetActive(text3 != "");
        choice4.SetActive(text4 != "");
        
        _choice1Text.text = text1;
        _choice2Text.text = text2;
        _choice3Text.text = text3;
        _choice4Text.text = text4;
    }
    
    void ChangeState(string text1, States state1, string text2, States state2,
        string text3, States state3, string text4, States state4)
    {
        ChangeTexts(text1, text2, text3, text4);
        _choice1Button.onClick.AddListener(()=>PanelButton(state1));
        _choice2Button.onClick.AddListener(()=>PanelButton(state2));
        _choice3Button.onClick.AddListener(()=>PanelButton(state3));
        _choice4Button.onClick.AddListener(()=>PanelButton(state4));
    }*/

        void OpponentESChange(int emotion)
        {
            int randDamage = Random.Range(-5, 5);
            int normalDamage = _playerDamage + randDamage;
            int strongDamage = normalDamage * strongMult;
            int damageDone = 0;
            int opponentES = _opponentValues.currentES;
            int opponentEmot = _opponentValues.emotionInt;
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
            

            if (GameManager.EmotionStrengths[opponentEmot][1] == emotion)
            {
                notifyText.text = $"It was super effective! You dealt {strongDamage} points of emotional strain " +
                                  $"to {_opponentValues.debaterName}.";
                opponentES -= strongDamage;
                damageDone = strongDamage;
            }
            else if (GameManager.EmotionStrengths[opponentEmot][2] == emotion)
            {
                notifyText.text = $"It was quite effective! You dealt {normalDamage} points of emotional strain " +
                                  $"to {_opponentValues.debaterName}.";
                opponentES -= normalDamage;
                damageDone = normalDamage;
            }
            else if (GameManager.EmotionStrengths[opponentEmot][3] == emotion)
            {
                notifyText.text = $"It wasn't very effective! You dealt {randDamage} points of emotional strain " +
                                  $"to {_opponentValues.debaterName}.";
                opponentES -= randDamage;
                damageDone = randDamage;
            }
            else
            {
                notifyText.text = $"It wasn't very effective! You dealt {randDamage} points of emotional strain " +
                                  $"to {_opponentValues.debaterName}.";
                opponentES -= randDamage;
                damageDone = randDamage;
            }
            
            if (GameManager.EmotionStrengths[opponentEmot][0] == emotion)
            {
                notifyText.text = $"It was ineffective! {_opponentValues.debaterName} regained {normalDamage} points " +
                                  $"of emotional stability.";
                opponentES += normalDamage;
                StartCoroutine(DamageGrow(normalDamage, emotColor));
            }
            else
            {
                StartCoroutine(DamageGrow(damageDone*-1, emotColor));
            }

            if (opponentES > _opponentValues.maxES)
            {
                opponentES = _opponentValues.maxES;
            }
            else if (opponentES < 0)
            {
                opponentES = 0;
            }
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
