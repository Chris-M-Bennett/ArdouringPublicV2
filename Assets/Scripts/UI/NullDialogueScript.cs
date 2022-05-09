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
        private Text _dialogue;
        // Start is called before the first frame update
        void Start()
        {
            if (GameManager.wasBoss)
            {
                overlay.SetActive(true);
                _dialogue = overlay.GetComponentInChildren<Text>();
                _dialogue.text = "";
                var areaOverloads = 0;
                var areaPacifies = 0;
                foreach (var status in GameManager.areaStatuses.statuses)
                {
                    switch (GameManager.areaStatuses.statuses[status])
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
                if(areaOverloads < areaPacifies || (areaOverloads == areaPacifies && state == 1))
                {
                    if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(2).name)
                    {
                        _dialogue.text = "Well, good for you. But do you really think this will change anything?";
                    }
                    else if(GameManager.overworld == SceneManager.GetSceneByBuildIndex(3).name)
                    {
                        _dialogue.text = "You know you can't win!";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(4).name)
                    {
                        _dialogue.text = "How boring you are.";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(5).name)
                    {
                        _dialogue.text = "You'll soon see how pointless this is!";
                    }
                }else if (areaOverloads > areaPacifies || (areaOverloads == areaPacifies && state == -1))
                {
                    if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(2).name)
                    {
                        _dialogue.text = "I didn't expect you to do that!";
                    }
                    else if(GameManager.overworld == SceneManager.GetSceneByBuildIndex(3).name)
                    {
                        _dialogue.text = "You're not acting like much of a hero are you?";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(4).name)
                    {
                        _dialogue.text = "Well done! Well done!.";
                    }
                    else if (GameManager.overworld == SceneManager.GetSceneByBuildIndex(5).name)
                    {
                        _dialogue.text = "You're worse than me!";
                    }
                }

                StartCoroutine(Speak());

            }
        }

        private IEnumerator Speak()
        {
            var chars = new char[0];
            foreach (var t in chars)
            {
                _dialogue.text += t;
                yield return new WaitForSeconds(0.1f);
            } 
            overlay.SetActive(false);
        }
    }
}
