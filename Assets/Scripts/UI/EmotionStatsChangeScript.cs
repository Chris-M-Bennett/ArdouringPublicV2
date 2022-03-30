using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EmotionStatsChangeScript : MonoBehaviour
    {
        [SerializeField] private DebateSystemScript debateSystem;
        private int _points = 5;
        private int _changeAmount;
        private int _happyCurrent;
        private int _sadCurrent;
        private int _angryCurrent;
        private int _proudCurrent;
        private int _afraidCurrent;

        private GameObject happyAddButton;
        private GameObject happyRemoveButton;
        private GameObject sadAddButton;
        private GameObject sadRemoveButton;
        private GameObject angryAddButton;
        private GameObject angryRemoveButton;
        private GameObject proudAddButton;
        private GameObject proudRemoveButton;
        private GameObject afraidAddButton;
        private GameObject afraidRemoveButton;
        private GameObject confirmButton;
        private Text pointsText;
        
        public Text happyStat;
        public Text sadStat;
        public  Text angryStat;
        public Text proudStat;
        public Text afraidStat;

        private void Start()
        {
            _happyCurrent = PlayerPrefs.GetInt("playerHappy");
            _sadCurrent = PlayerPrefs.GetInt("playerSad");
            _angryCurrent = PlayerPrefs.GetInt("playerAngry");
            _proudCurrent = PlayerPrefs.GetInt("playerProud");
            _afraidCurrent = PlayerPrefs.GetInt("playerAfraid");
            happyStat.text = _happyCurrent.ToString();
            sadStat.text = _sadCurrent.ToString();
            angryStat.text = _sadCurrent.ToString();
            proudStat.text = _sadCurrent.ToString();
            afraidStat.text = _sadCurrent.ToString();

            happyAddButton = transform.Find("Happy Add Button").gameObject;
            happyRemoveButton = transform.Find("Happy Remove Button").gameObject;
            sadAddButton = transform.Find("Sad Add Button").gameObject;
            sadRemoveButton = transform.Find("Sad Remove Button").gameObject;
            angryAddButton = transform.Find("Angry Add Button").gameObject;
            angryRemoveButton = transform.Find("Angry Remove Button").gameObject;
            proudAddButton = transform.Find("Proud Add Button").gameObject;
            proudRemoveButton = transform.Find("Proud Remove Button").gameObject;
            afraidAddButton = transform.Find("Afraid Add Button").gameObject;
            afraidRemoveButton = transform.Find("Afraid Remove Button").gameObject;
            confirmButton = transform.Find("Confirm Button").gameObject;

            pointsText = transform.Find("Points Text").GetComponent<Text>();
            pointsText.text = $"Points to Spend: {_points.ToString()}";
        }
        
        public void AlterStat(int emotCurrent, Text emotText, GameObject emotAddButton,
            GameObject emotRemoveButton, bool increase)
        {
            var emotValue = Int32.Parse(emotText.text);
            if (increase && emotValue < emotCurrent + 3)
            {
                emotValue++;
                _points--;
                
                emotRemoveButton.SetActive(true);
            } else if (!increase && emotValue > emotCurrent)
            {
                emotValue--;
                _points++;
                emotAddButton.SetActive(true);
            }
            emotText.text = emotValue.ToString();
            pointsText.text = $"Points to Spend: {_points.ToString()}";
            if (emotValue == emotCurrent + 3)
            {
                emotAddButton.SetActive(false);
            } else if (emotValue == emotCurrent)
            {
                emotRemoveButton.SetActive(false);
            }

            if (_points <= 0)
            {
                confirmButton.SetActive(true);
                happyAddButton.SetActive(false);
                sadAddButton.SetActive(false);
                angryAddButton.SetActive(false);
                proudAddButton.SetActive(false);
                afraidAddButton.SetActive(false);
            }else
            {
                confirmButton.SetActive(false);
            }
        }
        
        public void IncreaseHappy(){
            AlterStat(_happyCurrent, happyStat, happyAddButton, happyRemoveButton,true);
        }
        
        public void DecreaseHappy(){
            AlterStat(_happyCurrent, happyStat, happyAddButton, happyRemoveButton,false);
        }
        
        public void IncreaseSad(){
            AlterStat(_sadCurrent, sadStat, sadAddButton, sadRemoveButton,true);
        }
        
        public void DecreaseSad(){
            AlterStat(_sadCurrent, sadStat, sadAddButton, sadRemoveButton,false);
        }
        
        public void IncreaseAngry(){
            AlterStat(_angryCurrent, angryStat, angryAddButton, angryRemoveButton,true);
        }
        
        public void DecreaseAngry(){
            AlterStat(_angryCurrent, angryStat, angryAddButton, angryRemoveButton,false);
        }
        
        public void IncreaseProud(){
            AlterStat(_proudCurrent, proudStat, proudAddButton, proudRemoveButton,true);
        }
        
        public void DecreaseProud(){
            AlterStat(_proudCurrent, proudStat, proudAddButton, proudRemoveButton,false);
        }
        
        public void IncreaseAfraid(){
            AlterStat(_afraidCurrent, afraidStat, afraidAddButton, afraidRemoveButton,true);
        }
        
        public void DecreaseAfraid(){
            AlterStat(_afraidCurrent, afraidStat, afraidAddButton, afraidRemoveButton,false);
        }

        public void ConfirmChoices()
        {
            PlayerPrefs.SetInt("playerHappy", Int32.Parse(happyStat.text));
            PlayerPrefs.SetInt("playerSad", Int32.Parse(sadStat.text));
            PlayerPrefs.SetInt("playerAngry", Int32.Parse(angryStat.text));
            PlayerPrefs.SetInt("playerProud", Int32.Parse(proudStat.text));
            PlayerPrefs.SetInt("playerAfraid", Int32.Parse(afraidStat.text));
            debateSystem.confirmExit = true;
        }
        /*public void DecreaseHappy()
        {
            var happyValue = Int32.Parse(happyStat.text);
            if (happyValue > _happyCurrent)
            {
                happyValue--;
                happyStat.text = happyValue.ToString();
                happyAddButton.gameObject.SetActive(true);
            }

            if (happyValue == _happyCurrent)
            {
                happyRemoveButton.gameObject.SetActive(false);
            }
        }*/
    }
}
