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
        [SerializeField] GameObject playerPanel;
    
        public void SetHud(DebateValuesScript debater){
            nameText.text = debater.debaterName;
            esText.text = $"Emotional Strain: {debater.currentES}";
            esSlider.value = debater.currentES;
            esSlider.maxValue = debater.maxES;
            if(gameObject == playerPanel)
            {
                levelText.text = "Level: "+PlayerPrefs.GetInt("playerLevel",1);
            }else
            {
                levelText.text = "Level: "+debater.debaterLevel;
            }
        }

        public void SetES(DebateValuesScript debater){
            esText.text = $"Emotional Strain: {debater.currentES}";
            esSlider.value = debater.currentES;
        }
    }
}
