using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI{
    public class MenuScript : MonoBehaviour
    {
        private void Awake(){
            Time.timeScale = 1;
        }

        public void NewGame(){
            //GameManager.NewGame = true;
            SceneManager.LoadSceneAsync("Overworld");
        }

        public void QuitGame(){
            Application.Quit();
            //EditorApplication.isPlaying = false;
        }
    }
}
