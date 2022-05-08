using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            PlayerPrefs.SetInt("playerProud", 1);
            PlayerPrefs.SetInt("playerAfraid", 1);
            PlayerPrefs.SetInt("playerLevel", 1);
            PlayerPrefs.SetInt("playerES", 100);
            SceneManager.LoadScene(1);
        }
        
        public void LoadGame(){
            foreach (var status in GameManager.areaStatuses.statuses)
            {
                GameManager.movedArea = false;
                GameManager.areaStatuses.statuses[status] = 0;
            }
            SceneManager.LoadScene(GameManager.overworld);
        }

        public void QuitGame(){
            //EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
