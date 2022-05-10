using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class NullDialogueScript : MonoBehaviour
    {
        [SerializeField] private GameObject overlay;
        [SerializeField] private LastOpponentTracker tracker;
        private Text _dialogueText;
        private string _dialogueString;
        // Start is called before the first frame update
        void Start()
        {
            if (GameManager.wasBoss)
            {
                overlay.SetActive(true);
                _dialogueText = overlay.GetComponentInChildren<Text>();
                _dialogueText.text = "";
                var areaOverloads = 0;
                var areaPacifies = 0;
                foreach (var t in GameManager.areaStatuses.statuses)
                {
                    switch (t)
                    {
                        case -1:
                            areaOverloads++;
                            break;
                        case 1:
                            areaPacifies++;
                            break;
                    }
                }
                var state = GameManager.areaStatuses.statuses[tracker.LastOpponent];
                var equal = areaOverloads == areaPacifies && state == 1;
                Debug.Log(state);
                if(areaOverloads < areaPacifies || equal)
                {
                    if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(2).name)
                    {
                        _dialogueString = "Well, good for you. But do you really think this will change anything?";
                    }
                    else if(GameManager.overworld == SceneManager.GetSceneByBuildIndex(3).name)
                    {
                        _dialogueString = "You know you can't win!";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(4).name)
                    {
                        _dialogueString = "How boring you are.";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(5).name)
                    {
                        _dialogueString = "You'll soon see how pointless this is!";
                    }
                }else if (areaOverloads > areaPacifies || !equal)
                {
                    if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(2).name)
                    {
                        _dialogueString = "I didn't expect you to do that!";
                    }
                    else if(GameManager.overworld == SceneManager.GetSceneByBuildIndex(3).name)
                    {
                        _dialogueString = "You're not acting like much of a hero are you?";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(4).name)
                    {
                        _dialogueString = "Well done! Well done!.";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(5).name)
                    {
                        _dialogueString = "You're worse than me!";
                    }
                }
                StartCoroutine(Speak());

            }
        }

        private IEnumerator Speak()
        {
            var chars = _dialogueString.ToCharArray();
            foreach (var t in chars)
            {
                _dialogueText.text += t;
                yield return new WaitForSeconds(0.1f);
            }
            if (_dialogueText.text == _dialogueString)
            {
                yield return new WaitForSeconds(1.5f);
                overlay.SetActive(false);
            }
            
        }
    }
}
