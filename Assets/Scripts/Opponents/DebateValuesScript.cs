using UnityEngine;

namespace Opponents
{
    public class DebateValuesScript : MonoBehaviour  
    {
        [Header("Name of this debater")]public string debaterName;
        [Header("Level of this debater. DO NOT CHANGE ON PLAYER")]public int debaterLevel;
        [Header("Damage this debater does to their opponent's ES")]public int debaterDamage;
        [Header("Maximum amount of ES this debater can have")]public int maxES;
        [HideInInspector] public int currentES = 50;
        [Header("Number for the emotion this debater starts in. Happy=0 Angry=1 Sad=2 Anxiety=3")]public int emotionInt;
        [HideInInspector]public int playerExp;
        [Header("Thresholds Scriptable Object '{}' for this opponent. The player does not have one")]public OpponentThresholds emotionThresholds;

        private SpriteRenderer _spriteRenderer;

        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (CompareTag("Player")) {
                maxES = PlayerPrefs.GetInt("playerMax", 100);
                currentES = PlayerPrefs.GetInt("playerES", maxES);
                debaterLevel = PlayerPrefs.GetInt("level", 1);
            }
            emotionInt = Random.Range(0,3);
            if(emotionThresholds){
                GetComponent<SpriteRenderer>().sprite = emotionThresholds.emotionSprites[emotionInt];
            }
        }

        public void CheckThreshold(int prevES)
        {
            var thresh = emotionThresholds.thresholds;
            /*if(currentES > thresh[0] && currentES <= thresh[0])
            {
                emotionInt = Random.Range(0,3);
            }
            else if (currentES > maxES && currentES <= thresh[thresh.Count-1])
            {
                emotionInt = Random.Range(0,3);
            }else{*/
            for (int i = 1; i < thresh.Count-1; i++)
            {
                if (prevES >= thresh[i] && currentES < thresh[i])
                {
                    int prevEmot = emotionInt;
                    while(emotionInt == prevEmot){
                        emotionInt = Random.Range(0, 3);
                    }
                }
            }
            _spriteRenderer.sprite = emotionThresholds.emotionSprites[emotionInt];
        }
    }
}
