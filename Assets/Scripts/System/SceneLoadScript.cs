using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
   public class SceneLoadScript : MonoBehaviour
   {
      [SerializeField] SceneAsset sceneToLoad;
      private void OnTriggerEnter2D(Collider2D player)
      {
         if (player.CompareTag("Player"))
         {
            SceneManager.LoadSceneAsync(sceneToLoad.name);
         }
      }
   }
}
