using System.Collections;
using System.Collections.Generic;
using Opponents;
using UnityEngine;
using UnityEngine.UI;

public class DebateHUDScript : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Text esText;
    public Slider esSlider;
    
    public void SetHUD(DebateValuesScript debater){
        nameText.text = debater.debaterName;
        levelText.text = "Level: "+debater.debaterLevel;
        esText.text = $"Emotional Stability: {debater.currentES}";
        esSlider.value = debater.currentES;
        esSlider.maxValue = debater.maxES;
    }

    public void SetES(DebateValuesScript debater){
        esText.text = $"Emotional Stability: {debater.currentES}";
        esSlider.value = debater.currentES;
    }
}
