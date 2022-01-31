using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI{
    public class MenuScript : MonoBehaviour
    {

        public void NewGame(){
            GameManager.newGame = true;
            SceneManager.LoadSceneAsync("Overworld");
        }

        public void QuitGame(){
            Application.Quit();
            //EditorApplication.isPlaying = false;
        }

        public void ToggleTutorials(bool toggle)
        {
            PlayerPrefs.SetInt("Tutorials", Convert.ToInt32(toggle));
            GameManager.tutorials = toggle;
        }
    }
}
