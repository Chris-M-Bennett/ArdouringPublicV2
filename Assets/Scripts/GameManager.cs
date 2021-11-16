using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameObject CurrentOpponent;
    public static bool NewGame;
    public static bool Tutorials = true;

    public static Dictionary<int, int[]> EmotionStrengths = new Dictionary<int, int[]>();
    
    // Start is called before the first frame update

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            //Prevents preloaded gameobject being destroyed
            DontDestroyOnLoad(gameObject);
        }else
        {
            //Destroys duplicate game manager
            Destroy(gameObject);
        }
        EmotionStrengths.Add(0, new int[] {0, 1, 2});
        EmotionStrengths.Add(1, new int[] {1, 2, 3});
        EmotionStrengths.Add(2, new int[] {2, 3, 0});
        EmotionStrengths.Add(3, new int[] {3, 0, 1});
    }
}
