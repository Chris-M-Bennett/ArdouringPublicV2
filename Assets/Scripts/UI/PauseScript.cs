using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{

    public class PauseScript : MenuScript
    {
        private float prevScale = 1;
        [SerializeField] private GameObject title;
        [SerializeField] private Button[] butts;
        [SerializeField] private Toggle tutToggle;
        private void Start()
        {
            //DontDestroyOnLoad(gameObject);
            tutToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Tutorials", 1));
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (Time.timeScale == 0)
                {
                    Resume();
                }else
                {
                    Pause(); 
                }
            }
        }

        private void Pause()
        {
            GetComponent<Image>().enabled = true;
            title.SetActive(true);
            tutToggle.gameObject.SetActive(true);
            for (int i = 0 ; i < butts.Length ; i++)
            {
                butts[i].gameObject.SetActive(true);
            }
            prevScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void Resume()
        {
            GetComponent<Image>().enabled = false;
            title.SetActive(false);
            tutToggle.gameObject.SetActive(false);
            for (int i = 0 ; i < butts.Length ; i++)
            {
                butts[i].gameObject.SetActive(false);
            }
            Time.timeScale = prevScale;
        }
        
        public void ToggleTutorials(bool toggle)
        {
            PlayerPrefs.SetInt("Tutorials", Convert.ToInt32(toggle));
            GameManager.tutorials = toggle;
        }

        public void MainMenu()
        {
            //WriteFile();
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
