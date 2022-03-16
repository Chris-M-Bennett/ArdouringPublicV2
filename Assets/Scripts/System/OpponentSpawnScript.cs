using System.Collections;
using Opponents;
using UnityEngine;
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
        [SerializeField] private OpponentOverworldStatuses areaStatuses;
        public int id;
        private string _overworldTalk;
        private TextMesh _speechBubble;
        [SerializeField] private GameObject sceneLoader;

        private int defeatState = 0;

        private void Start()
        {
            if (GameManager.areaStatuses != areaStatuses)
            {
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
                Instantiate(defeatedOpponent, transform);
                _speechBubble = defeatedOpponent.GetComponentInChildren<TextMesh>();
                _overworldTalk = _speechBubble.text;
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
            _speechBubble.gameObject.SetActive(true);
            var chars = _overworldTalk.Split();
            for (int i = 0; i < chars.Length-1; i++)
            {
                _speechBubble.text += chars[i];
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
