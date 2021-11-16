using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MoveBarsScript : MonoBehaviour
    {
        private Transform leftBars;
        private Transform rightBars;

        [SerializeField] private float moveAmount;

        [SerializeField] private int endTrans = 0;
    
        private void Start()
        {
            leftBars = transform.GetChild(0);
            rightBars = transform.GetChild(1);
        }

        internal IEnumerator MoveThoseBars(bool transIn)
        {
            Debug.Log("Moving");
            while(leftBars.position.x != endTrans)
            {
                leftBars.Translate(moveAmount,0,0);
                rightBars.Translate(moveAmount,0,0);
                yield return new WaitForSeconds(0.01f);
            }

            if (!transIn)
            {
                SceneManager.LoadScene("Debate");
            }
            yield return null;
        }
    }
}
