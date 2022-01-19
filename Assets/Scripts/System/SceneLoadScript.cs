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
            SceneManager.LoadSceneAsync(sceneToLoad);
         }
      }
   }
}
