using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EmotionStatsChangeScript : MonoBehaviour
    {
        [SerializeField] private Text happyStat;
        [SerializeField] private Text sadStat;
        [SerializeField] private Text angryStat;
        [SerializeField] private Text proudStat;
        [SerializeField] private Text afraidStat;
        private int points;
        private int changeAmount;
        private int happyCurrent;
        private int sadCurrent;
        private int angryCurrent;
        private int proudCurrent;
        private int afraidCurrent;

        private void Start()
        {
            happyCurrent = PlayerPrefs.GetInt("playerHappy");
        }
        
        public void IncreaseHappy(){
            var happyValue = Int32.Parse(happyStat.text);
            if(happyValue < happyCurrent + 3)
            {
                happyValue ++;
                happyStat.text = happyValue.ToString();
            }
        }

    }
}
