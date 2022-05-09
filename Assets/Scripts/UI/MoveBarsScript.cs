using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MoveBarsScript : MonoBehaviour
    {
        [SerializeField] private float moveAmount;
        [SerializeField] private Transform endTrans;

        public IEnumerator MoveThoseBars(bool change, string scene)
        {

            while(transform.position.x < endTrans.position.x)
            {
                transform.Translate(moveAmount,0,0);
                yield return new WaitForSeconds(0.03f);
            }
            
            if (SceneManager.GetSceneByName(scene) != default)
            {
                Debug.LogError("Either no scene with the supplied name exists or no scene name was supplied");
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }

        public IEnumerator MoveThoseBars(bool change)
        {

            while(transform.position.x < endTrans.position.x)
            {
                transform.Translate(moveAmount,0,0);
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
}
