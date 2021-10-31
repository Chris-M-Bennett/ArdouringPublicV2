using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI{
    public class MenuScript : MonoBehaviour
    {
        public void NewGame(){
            GameManager.newGame = true;
            SceneManager.LoadSceneAsync("Overworld");
        }
        
        public void MainMenu(){
            SceneManager.LoadSceneAsync("MainMenu");
        }
    
        public void QuitGame(){
            Application.Quit();
            EditorApplication.isPlaying = false;
        }
    }
}
