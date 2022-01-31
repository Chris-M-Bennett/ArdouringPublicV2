using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{

    public class PauseScript : MenuScript
    {
        [SerializeField] private Toggle tutToggle;
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            tutToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Tutorials", 1));
        }

        public void MainMenu(){
            SceneManager.LoadSceneAsync("MainMenu");
            
        }
    }
}
