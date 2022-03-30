using System;
using UnityEngine;

namespace Opponents
{
    public class DebateValuesScript : MonoBehaviour  
    {
        [Header("Mouse over field names for description of what to add")]
        [Tooltip("Name of this debater")]public string debaterName;
        [Tooltip("Level of this debater")]public int debaterLevel;
        [Tooltip("Damage this debater does to their opponent's ES")]public int debaterDamage;
        [Tooltip("Maximum amount of ES this debater can have")]public int maxES;
        public int startES = 100;
        public int currentES = 100;
        [Tooltip("Amounts of each emotion opponent starts with")]public int[] emotAmounts = new int[5];

        private void OnValidate()
        {
            var i = 0;
            foreach (var emotion in emotAmounts)
            {
                if (emotion == 0)
                {
                    emotAmounts[i] = 1;
                }
                i++;
            }
        }
    }
}
