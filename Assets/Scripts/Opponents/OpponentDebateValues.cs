using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Opponents
{
    [RequireComponent(typeof(Animator))]
    public class OpponentDebateValues : DebateValuesScript
    {
        [Tooltip("Thresholds Scriptable Object '{}' for this opponent. Create from Assets task menu."+
                 " The player does not have one")]public OpponentThresholds emotionThresholds;

        private List<int> thresh;
        private List<int> myEmotions;
        [Tooltip("Number for the emotion this debater starts in. Happy, Angry, Sad, Confident or Afraid.")]
        public int emotionInt;

        private Animator _animator;
        private static readonly int EmotionInt = Animator.StringToHash("EmotionInt");
        [HideInInspector] public int prevES;

        /* void OnValidate()
        {
            if (!emotionThresholds.emotions.Contains(emotionInt))
            {
                emotionInt = myEmotions[0];
                Debug.LogError("Given emotion is invalid.");
            }
        }*/
    
        // Start is called before the first frame update
        void Awake()
        {
            _animator = GetComponent<Animator>();
            if(emotionThresholds)
            {
                myEmotions = emotionThresholds.emotions;
                thresh = emotionThresholds.thresholds;
                var rand = Random.Range(0, myEmotions.Count - 1);
                emotionInt = myEmotions[rand];
                _animator.SetInteger(EmotionInt, emotionInt);
            }
        }

        public void CheckThreshold(int currentES)
        {
            for (int i = 0; i < thresh.Count; i++)
            {
                if ((currentES > thresh[i] && prevES < thresh[i]) || (currentES < -thresh[i] && prevES > thresh[i]))
                {
                    //List<int> validEmots = myEmotions.Remove(emotionInt);
                    var rand = Random.Range(0, myEmotions.Count);
                    emotionInt = myEmotions[rand];
                    _animator.SetInteger(EmotionInt, emotionInt);
                }
            }
        }
    }
}
