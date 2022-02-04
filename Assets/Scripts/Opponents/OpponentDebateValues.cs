using System.Collections.Generic;
using UnityEngine;

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

       /* void OnValidate()
        {
            if (!emotionThresholds.emotions.Contains(emotionInt))
            {
                emotionInt = myEmotions[0];
                Debug.LogError("Given emotion is invalid.");
            }
        }*/
    
        // Start is called before the first frame update
        void Start()
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
            for (int i = 1; i < thresh.Count; i++)
            {
                if (currentES > thresh[i] || currentES < -thresh[i])
                {
                    var rand = Random.Range(0, myEmotions.Count - 1);
                    while (rand == emotionInt)
                    {
                        rand = Random.Range(0, myEmotions.Count - 1);
                    }
                    Debug.Log(rand);
                    emotionInt = myEmotions[rand];
                    _animator.SetInteger(EmotionInt, emotionInt);

                }
            }
        }
    }
}
