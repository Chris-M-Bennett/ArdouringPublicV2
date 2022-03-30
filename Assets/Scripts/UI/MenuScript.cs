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
            SceneManager.LoadSceneAsync(1);
        }

        public void QuitGame(){
            WriteFile();
            Application.Quit();
            //EditorApplication.isPlaying = false;
        }

        internal void WriteFile(){
            
            string path = Application.dataPath + "/Metric Data";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            StreamWriter writer = new StreamWriter(path+$"/{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.json",true);
            writer.WriteLine($"No. times happiness area was entered: {GameManager.happyEnterCount}\n");
            writer.WriteLine($"No. times sadness area was entered: {GameManager.sadEnterCount}\n");
            writer.WriteLine($"No. times angry area was entered: {GameManager.angryEnterCount}\n");
            writer.WriteLine($"No. times proud area was entered: {GameManager.proudEnterCount}\n");
            writer.WriteLine($"No. times player lost in happiness area: {GameManager.happyDeathCount}\n");
            writer.WriteLine($"No. times player lost in sadness area: {GameManager.sadDeathCount}\n");
            writer.WriteLine($"No. times player lost in angry area: {GameManager.angryDeathCount}\n");
            writer.WriteLine($"No. times player lost in proud area: {GameManager.proudDeathCount}\n");
            writer.WriteLine($"No. times player defeated an opponent: {GameManager.defeatingCount}\n");
            writer.WriteLine($"No. times player overloaded an opponent: {PlayerPrefs.GetInt("Overloads",0)}\n");
            writer.WriteLine($"No. times player pacified an opponent: {PlayerPrefs.GetInt("Pacifies",0)}\n");
            int i = 0;
            foreach (var debate in GameManager.debateTimes)
            {
                writer.WriteLine($"Times spent in debate {i+1}: {debate[0]} against {debate[1]}");  
                i++;
            }
            writer.Close();
        }
    }
}
