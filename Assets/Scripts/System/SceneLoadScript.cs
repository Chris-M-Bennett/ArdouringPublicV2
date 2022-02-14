using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
   public class SceneLoadScript : MonoBehaviour
   {
      [SerializeField] private string sceneToLoad;
      private void OnTriggerEnter2D(Collider2D player)
      {
         if (player.CompareTag("Player"))
         {
            GameManager.MovedArea = true;
            SceneManager.LoadSceneAsync(sceneToLoad);
         }
      }
   }
}
