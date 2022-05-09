using TMPro;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System
{
   public class SceneLoadScript : MonoBehaviour
   {
      private MoveBarsScript _transBars;
      [SerializeField] private string sceneToLoad;

      private void Start()
      {
         _transBars = GameObject.FindWithTag("Transition Out").GetComponent<MoveBarsScript>();
      }

      private void OnTriggerEnter2D(Collider2D player)
      {
         if (player.CompareTag("Player"))
         {
            StartCoroutine(_transBars.MoveThoseBars(true, sceneToLoad));
         }
      }
   }
}
