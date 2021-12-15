using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI{
    public class MenuScript : MonoBehaviour
    {
        [SerializeField] private Toggle tutToggle;
        private void Start()
        {
            tutToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Tutorials", 1));
        }

        public void NewGame(){
            GameManager.NewGame = true;
            SceneManager.LoadSceneAsync("Overworld");
        }
        
        public void MainMenu(){
            SceneManager.LoadSceneAsync("MainMenu");
        }
    
        public void QuitGame(){
            Application.Quit();
            //EditorApplication.isPlaying = false;
        }

        public void ToggleTutorials(bool toggle)
        {
            PlayerPrefs.SetInt("Tutorials", Convert.ToInt32(toggle));
            GameManager.Tutorials = toggle;
        }
    }
}
