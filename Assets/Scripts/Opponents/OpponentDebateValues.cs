using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        private TextMeshPro speechBubble;
        [HideInInspector] public int prevES;
        
        [SerializeField,TextArea,Tooltip("The bark said by the opponent at the start of a debate")]
        private string openingLine;
        [SerializeField,TextArea,Tooltip("The bark said by the opponent at the end of a debate if they have been overloaded")]
        private string overloadedLine;
        [SerializeField,TextArea,Tooltip("The bark said by the opponent at the end of a debate if they have been overloaded")]
        private string pacifiedLine;
        
        
    
        // Start is called before the first frame update
        void Awake()
        {
            _animator = GetComponent<Animator>();
            speechBubble = GetComponentInChildren<TextMeshPro>();
            speechBubble.transform.parent.gameObject.SetActive(false);
            if (emotionThresholds != null)
            {
                    //myEmotions = emotionThresholds.emotions;
                    thresh = emotionThresholds.thresholds;
                    //emotionInt = myEmotions[rand];
                    emotionEnum = emotionThresholds.ChangeOpponentEmot(new Emotions());
                    _animator.SetInteger(EmotionEnum, (int)emotionEnum);
            }
        }

        public void Speak(Stages stage)
        {
            speechBubble.transform.parent.gameObject.SetActive(true);
            if (stage == Stages.Opening)
            {
                speechBubble.text = openingLine;
            }else if (stage == Stages.Overloaded)
            {
                speechBubble.text = overloadedLine;
            }else if (stage == Stages.Pacified)
            {
                speechBubble.text = pacifiedLine;
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

public enum Stages
{
    Opening = 0,
    Overloaded = 1,
    Pacified = 2,
    
}