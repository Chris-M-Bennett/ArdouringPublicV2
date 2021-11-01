using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebateValuesScript : MonoBehaviour  
{
     public string debaterName;
     public int debaterLevel;
     public int debaterDamage;
     public int maxES;
     public int currentES;
     public int emotionInt;
     public int playerExp;
     public OpponentThresholds emotionThresholds;
     // Start is called before the first frame update
    void Start()
    {
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
    
    void CheckThreshold()
    {
        var thresh = emotionThresholds.thresholds;
        if(currentES > 0 && currentES <= thresh[0])
        {
            emotionInt = Random.Range(0,3);
        }
        else if (currentES > maxES && currentES <= thresh[thresh.Count-1])
        {
            emotionInt = Random.Range(0,3);
        }else{
            for (int i = 1; i < thresh.Count-1; i++)
            {
                if(currentES > thresh[i] && currentES <= thresh[i+1])
                    emotionInt = Random.Range(0,3);
            }
        }
    }
}
