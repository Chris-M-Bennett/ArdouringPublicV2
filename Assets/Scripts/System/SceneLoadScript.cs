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
            if(sceneToLoad == "Happiness Area")
            {
               GameManager.happyEnterCount += 1;
            } else if (sceneToLoad == "Sadness Area")
            {
               GameManager.sadEnterCount += 1;
            } else if (sceneToLoad == "Angry Area")
            {
               GameManager.angryEnterCount += 1;
            } else if (sceneToLoad == "Proud Area")
            {
               GameManager.proudEnterCount += 1;
            }
            GameManager.movedArea = true;
            SceneManager.LoadSceneAsync(sceneToLoad);
         }
      }
   }
}
