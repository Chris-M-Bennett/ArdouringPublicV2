using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EmotionStatsChangeScript : MonoBehaviour
    {
        [SerializeField] private Text emotStat;
        [SerializeField] private string emotName;
        [SerializeField] private bool add;
        private int changeAmount;
        private int current;

        private void Start()
        {
            current = PlayerPrefs.GetInt(emotName, 1);
            emotStat.text = current.ToString();
        }

        public void ChangeEmotAmount()
        {
            if (add && changeAmount < 3)
            {
                changeAmount++;
            }
            else if (!add && changeAmount >= 0)
            {
                changeAmount--;
            }
            emotStat.text = $"{current+changeAmount}";
        }

    }
}
