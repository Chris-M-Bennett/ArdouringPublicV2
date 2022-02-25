using System;
using UnityEngine;

namespace UI
{
    public class EmotionChangeButtonScript : MonoBehaviour
    {
        [SerializeField] private string amountName;
        [SerializeField] private bool add;
        [SerializeField] private DebateSystemScript debateSystem;
        private int changeAmount;
        void ChangeEmotAmount()
        {
            if (add && changeAmount < 3)
            {
                changeAmount++;
            }
            else if (!add && changeAmount >= 0)
            {
                changeAmount--;
            }
        }

    }
}
