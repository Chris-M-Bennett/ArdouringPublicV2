using System;
using Opponents;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DebateHUDScript : MonoBehaviour
    {
        public Text nameText;
        public Text levelText;
        [HideInInspector] public Text _esText;
        [HideInInspector] public Slider _esSlider;
        [SerializeField] GameObject playerPanel;

        private void Awake(){
            _esSlider = gameObject.transform.GetComponentInChildren<Slider>();
            _esText = _esSlider.transform.Find("Emotional Stability").GetComponent<Text>();
        }

        public void SetHud(DebateValuesScript debater){
            nameText.text = debater.debaterName;
            _esText.text = $"Emotional Strain: {debater.currentES}";
            _esSlider.value = debater.currentES;
            _esSlider.maxValue = debater.maxES;
            if(gameObject == playerPanel)
            {
                levelText.text = "Level: "+PlayerPrefs.GetInt("playerLevel",1);
            }else
            {
                levelText.text = "Level: "+debater.debaterLevel;
            }
        }

        public void SetES(DebateValuesScript debater){
            _esText.text = $"Emotional Strain: {debater.currentES}";
            _esSlider.value = debater.currentES;
        }
    }
}
