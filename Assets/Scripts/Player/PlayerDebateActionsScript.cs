using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player{
    public partial class PlayerDebateActionsScript : MonoBehaviour
    {
        [SerializeField] private Text notifyText;
        [SerializeField] private DebateHUDScript opponentHUD;
        [SerializeField] private DebateSystemScript debateSystem;
        private GameObject opponentGO;
        private static DebateValuesScript _opponentValues;
        private static int _playerDamage;
        private DebateState _turnState;

        public bool playerHadTurn;

        private void Start()
        {
            debateSystem = GetComponentInParent<DebateSystemScript>();
            opponentGO = GetComponentInParent<DebateSystemScript>().opponentGO;
            if (opponentGO.GetComponent<DebateValuesScript>())
            {
                _opponentValues = opponentGO.GetComponent<DebateValuesScript>();
            }
            else
            {
                Debug.LogError("Supplied Game Object is not an opponent");
            }

            _playerDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<DebateValuesScript>().debaterDamage;
        }


        /*void SetButtonUnlock(bool state)
    {
        _choice4Button.enabled = state;
        choice4.GetComponent<Image>().color = state ? Color.white : Color.gray;
    }*/

        private void CheckPlayerTurn(int emotion){
            if(debateSystem.state == DebateState.Player){
                OpponentESChange(emotion);
            }
        }
        public void HappyButton()
        {
            CheckPlayerTurn(0);
        }

        public void AngryButton()
        {
            CheckPlayerTurn(1);
        }

        public void SadButton()
        {
            CheckPlayerTurn(2);
        }

        public void AnxiousButton()
        {
            CheckPlayerTurn(3);
        }

        public void RunButton()
        {
            if(debateSystem.state == DebateState.Player){
                StartCoroutine(debateSystem.EndDebate());
                //SceneManager.LoadSceneAsync("Overworld");
            }
        }

        /*void ChangeTexts(string text1, string text2, string text3, string text4){
       //Removes listeners previous added to buttons
        _choice1Button.onClick.RemoveAllListeners();
        _choice2Button.onClick.RemoveAllListeners();
        _choice3Button.onClick.RemoveAllListeners();
        _choice4Button.onClick.RemoveAllListeners();
        
        //Checks text for buttons to see if they should be active
        choice1.SetActive(text1 != "");
        choice2.SetActive(text2 != "");
        choice3.SetActive(text3 != "");
        choice4.SetActive(text4 != "");
        
        _choice1Text.text = text1;
        _choice2Text.text = text2;
        _choice3Text.text = text3;
        _choice4Text.text = text4;
    }
    
    void ChangeState(string text1, States state1, string text2, States state2,
        string text3, States state3, string text4, States state4)
    {
        ChangeTexts(text1, text2, text3, text4);
        _choice1Button.onClick.AddListener(()=>PanelButton(state1));
        _choice2Button.onClick.AddListener(()=>PanelButton(state2));
        _choice3Button.onClick.AddListener(()=>PanelButton(state3));
        _choice4Button.onClick.AddListener(()=>PanelButton(state4));
    }*/

        void OpponentESChange(int emotion)
        {
            var opponentES = _opponentValues.currentES;
            var opponentEmot = _opponentValues.emotionInt;
            if (GameManager.EmotionStrengths[opponentEmot] == emotion)
            {
                notifyText.text = "It was super effective";
                opponentES -= _playerDamage * global::PlayerDebateActionsScript.NORMAL_MULT;
            }
            else
            {
                notifyText.text = "It was ineffective";
                opponentES += _playerDamage * global::PlayerDebateActionsScript.NORMAL_MULT;
            }

            if (opponentES > _opponentValues.maxES)
            {
                opponentES = _opponentValues.maxES;
            }
            else if (opponentES < 0)
            {
                opponentES = 0;
            }
            _opponentValues.currentES = opponentES;
            opponentHUD.SetES(opponentES);
            debateSystem.PlayerHadTurn = true;
        }
    }
}
