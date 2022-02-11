using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameObject DebateOpponent;
    //public static bool wasPacified = false;
    public static OpponentOverworldStatuses AreaStatuses;
    public static bool NewGame = true;
    public static bool MovedArea;
    public static bool Tutorials = true;
    public static Sprite DebateBG;

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
        if (NewGame)
        {
            PlayerPrefs.SetInt("Overloads", 0);
            PlayerPrefs.SetInt("Pacifies", 0);
        }
    }
}
