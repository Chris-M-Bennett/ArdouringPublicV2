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
        public int ID;
        [SerializeField] private GameObject SceneLoader;

        private int defeatState = 0;

        private void Start()
        {
            if (AreaStatuses != areaStatuses)
            {
                AreaStatuses = areaStatuses;
            }
            if (NewGame)
            {
                AreaStatuses.Reset();
                NewGame = false;
            }
            defeatState = areaStatuses.statuses[ID];
            AreaStatuses.statuses[ID] = defeatState;
            if (defeatState > 0)
            {
                Instantiate(defeatedOpponent, transform);
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
                if(SceneLoader != null)
                {
                    SceneLoader.SetActive(false);
                }
            }
        }
    }
}
