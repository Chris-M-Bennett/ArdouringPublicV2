using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
   public class SceneLoadScript : MonoBehaviour
   {
      [SerializeField] string sceneToLoad;
      private void OnTriggerEnter2D(Collider2D player)
      {
         if (player.CompareTag("Player"))
         {
            /*if (sceneToLoad == "HubArea")
            {
               Vector2 pos = player.transform.position;
               PlayerPrefs.SetFloat("playerXPos", pos.x-5);
               PlayerPrefs.SetFloat("playerYPos", pos.y);
            }*/
            SceneManager.LoadSceneAsync(sceneToLoad);
         }
      }
   }
}
