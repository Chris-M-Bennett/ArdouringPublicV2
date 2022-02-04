using Opponents;
using UnityEngine;

namespace Player
{
    public class PlayerDebateValues : DebateValuesScript
    {
        // Start is called before the first frame update
        void Start()
        {
            maxES = PlayerPrefs.GetInt("playerMax", 100);
            currentES = PlayerPrefs.GetInt("playerES", maxES);
            debaterLevel = PlayerPrefs.GetInt("level", 1);
                
            emotAmounts[0] = PlayerPrefs.GetInt("playerHappy", 2);
            emotAmounts[1] = PlayerPrefs.GetInt("playerSad", 2);
            emotAmounts[2] = PlayerPrefs.GetInt("playerAngry", 2);
            emotAmounts[3] = PlayerPrefs.GetInt("playerConfident", 2);
            emotAmounts[4] = PlayerPrefs.GetInt("playerAfraid", 2);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
