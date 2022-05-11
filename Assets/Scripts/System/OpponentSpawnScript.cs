using System.Collections;
using Opponents;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using static GameManager;

namespace System
{
    public class OpponentSpawnScript : MonoBehaviour
    {
        [SerializeField] private GameObject liveOpponent;
        [SerializeField] private GameObject defeatedOpponent;
        [SerializeField, Tooltip("The movement point the opponent should by moving away from")] private DirectOverworldMovementScript lastDest;
        [SerializeField, Tooltip("The movement point the spawned live opponent should by moving towards")]
        private DirectOverworldMovementScript currentDest;
        [SerializeField] private Vector2 offSet;
        [SerializeField] public OpponentOverworldStatuses areaStatuses;
        public int id;
        private string _overworldTalk;
        private TextMeshPro _speech;
        [SerializeField] private GameObject sceneLoader;
        [SerializeField, Tooltip("Whether the opponent is the boss of the area")]
        public bool isBoss;
        [SerializeField] private NullDialogueScript nullChecker;
        
        [HideInInspector] public Transform speechBubble;
        private int defeatState = 0;

        private void Start()
        {
            if (GameManager.areaStatuses != areaStatuses)
            {
                /*
                if (GameManager.loadFromSave)
                {
                    areaStatuses = GameManager.areaStatuses;
                    GameManager.loadFromSave = false;
                }
                else
                {
                    GameManager.areaStatuses = areaStatuses;
                }*/
                //areaStatuses = GameManager.areaStatuses;
                GameManager.areaStatuses = areaStatuses;
            }
            if (newGame)
            {
                GameManager.areaStatuses.Reset();
                newGame = false;
            }
            defeatState = areaStatuses.statuses[id];
            GameManager.areaStatuses.statuses[id] = defeatState;
            if (defeatState > 0)
            {
                speechBubble = Instantiate(defeatedOpponent, transform).transform.GetChild(0);
                _speech = speechBubble.GetComponentInChildren<TextMeshPro>();
                _overworldTalk = _speech.text;
                _speech.text = "";
                speechBubble.gameObject.SetActive(false);
            }
            else if (defeatState == 0)
            {
                var pos = transform.position;
                var live = Instantiate(liveOpponent, new Vector3(pos.x+offSet.x,pos.y+offSet.y,0f),Quaternion.identity,transform);
                if (live.GetComponent<OpponentOverworldScript>()){
                    var myComponent = live.GetComponent<OpponentOverworldScript>();
                    myComponent.LastDest = lastDest;
                    myComponent.CurrentDest = currentDest;
                }
                if(sceneLoader != null)
                {
                    sceneLoader.SetActive(false);
                }
            }
        }

        public IEnumerator Speak(){
            speechBubble.gameObject.SetActive(true);
            var chars = _overworldTalk.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                _speech.text += chars[i];
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
