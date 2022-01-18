using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameObject debateOpponent;
    public static bool wonDebate = false;
    public static bool newGame;
    public static bool tutorials = true;
    public static Sprite debateBG;

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
        EmotionStrengths.Add(0, new[] {0, 1, 2, 3});
        EmotionStrengths.Add(1, new[] {1, 2, 3, 4});
        EmotionStrengths.Add(2, new[] {2, 3, 4, 0});
        EmotionStrengths.Add(3, new[] {3, 4, 0, 1});
        EmotionStrengths.Add(4, new[] {4, 0, 1, 2});
    }
}
