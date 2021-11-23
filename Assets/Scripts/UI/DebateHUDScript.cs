using Opponents;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DebateHUDScript : MonoBehaviour
    {
        public Text nameText;
        public Text levelText;
        public Text esText;
        public Slider esSlider;
    
        public void SetHUD(DebateValuesScript debater){
            nameText.text = debater.debaterName;
            levelText.text = "Level: "+PlayerPrefs.GetInt("playerLevel",0);
            esText.text = $"Emotional Stability: {debater.currentES}";
            esSlider.value = debater.currentES;
            esSlider.maxValue = debater.maxES;
        }

        public void SetES(DebateValuesScript debater){
            esText.text = $"Emotional Stability: {debater.currentES}";
            esSlider.value = debater.currentES;
        }
    }
}
