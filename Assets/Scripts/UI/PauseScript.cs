using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{

    public class PauseScript : MenuScript
    {
        private float prevScale = 1;
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
                Pause();
            }
        }

        private void Pause()
        {
            for (int i = 0 ; i < transform.childCount ; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            GetComponent<Image>().enabled = true;
            prevScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void Resume()
        {
            Debug.Log(transform.childCount);
            for (int i = 0 ; i < transform.childCount ; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            GetComponent<Image>().enabled = false;
            Time.timeScale = prevScale;
        }
        
        public void ToggleTutorials(bool toggle)
        {
            PlayerPrefs.SetInt("Tutorials", Convert.ToInt32(toggle));
            GameManager.tutorials = toggle;
        }

        public void MainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
            
        }
    }
}
