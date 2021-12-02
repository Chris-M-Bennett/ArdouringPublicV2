using UnityEngine;
using Random = UnityEngine.Random;

namespace Opponents
{
    public class DirectOverworldMovementScript : MonoBehaviour
    {
        [SerializeField] private DirectOverworldMovementScript[] nextPoints;
        [SerializeField] private bool isEnd = false;
        

        private void OnTriggerEnter2D(Collider2D col)
        {
            var ent = col.gameObject.GetComponent<OpponentOverworldScript>();
            
            if (!ent || ent.lastDest == null)
            {
                if (!col.gameObject.CompareTag("Player"))
                {
                    Debug.LogError("This should not be null");
                }
                return;
            }

            var from = ent.lastDest;
            var to = nextPoints[Random.Range(0, nextPoints.Length)];

            if (to == from && !isEnd)
            {
                while (to == from)
                {
                    to = nextPoints[Random.Range(0, nextPoints.Length)];
                }
            }

            ent.MoveMe(to);
        }
    }
}
