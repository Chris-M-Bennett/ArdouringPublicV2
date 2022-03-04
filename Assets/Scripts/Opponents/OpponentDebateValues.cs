using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Opponents
{
    [RequireComponent(typeof(Animator))]
    public class OpponentDebateValues : DebateValuesScript
    {
        [Tooltip("Thresholds Scriptable Object '{}' for this opponent. Create from Assets task menu."+
                 " The player does not have one")]
        public OpponentThresholds emotionThresholds;

        private List<int> thresh;
        private List<int> myEmotions;
        [Tooltip("Number for the emotion this debater starts in. Happy, Angry, Sad, Confident or Afraid.")]
        //public int emotionInt;

        public Emotions emotionEnum;

        private Animator _animator;
        private static readonly int EmotionEnum = Animator.StringToHash("EmotionInt");
        [HideInInspector] public int prevES;
        
    
        // Start is called before the first frame update
        void Awake()
        {
            _animator = GetComponent<Animator>();
            if (emotionThresholds != null)
            {
                    //myEmotions = emotionThresholds.emotions;
                    thresh = emotionThresholds.thresholds;
                    //emotionInt = myEmotions[rand];
                    emotionEnum = emotionThresholds.ChangeOpponentEmot(new Emotions());
                    _animator.SetInteger(EmotionEnum, (int)emotionEnum);
            }
        }

        public void CheckThreshold(int currentES)
        {
            var validEmots = new List<int>();
            foreach (var i in thresh)
            {
                if ((currentES > i && prevES < i) || (currentES < i && prevES > i))
                {
                    emotionEnum = emotionThresholds.ChangeOpponentEmot(emotionEnum);
                }
            }
            // for (int i = 0; i < thresh.Count; i++)
            // {
            //     if ((currentES > thresh[i] && prevES < thresh[i]) || (currentES < -thresh[i] && prevES > thresh[i]))
            //     {
            //
            //         emotionEnum = emotionThresholds.GiveMeAnotherEmotion(emotionEnum);
            //
            //         // for (int j=0; j < myEmotions.Count-1; j++)
            //         // {
            //         //     if (myEmotions[j] != myEmotions.IndexOf(emotionInt))
            //         //     {
            //         //         validEmots.Add(myEmotions[j]);
            //         //     }
            //         // }
            //         //
            //         //emotionInt = validEmots[Random.Range(0, validEmots.Count-1)];
            //         _animator.SetInteger(EmotionEnum, (int)emotionEnum);
            //     }
            // }
        }
        
    }
}


public enum Emotions
{
    Happy = 0,
    Sad = 1,
    Angry = 3,
    Proud = 4,
    Afraid = 5
}