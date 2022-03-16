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
            GameManager.newGame = true;
            PlayerPrefs.SetInt("Overloads", 0);
            PlayerPrefs.SetInt("Pacifies", 0);
            PlayerPrefs.SetInt("playerExp", 0);
            PlayerPrefs.SetInt("playerHappy", 1);
            PlayerPrefs.SetInt("playerSad", 1);
            PlayerPrefs.SetInt("playerAngry", 1);
            PlayerPrefs.SetInt("playerConfident", 1);
            PlayerPrefs.SetInt("playerAfraid", 1);
            PlayerPrefs.SetInt("playerLevel", 1);
            PlayerPrefs.SetInt("playerES", 100);
            SceneManager.LoadSceneAsync("Overworld");
        }

        public void QuitGame(){
            Application.Quit();
            //EditorApplication.isPlaying = false;
        }
    }
}
