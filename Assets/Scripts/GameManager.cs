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

    public static Dictionary<Emotions, int[]> EmotionStrengths = new Dictionary<Emotions, int[]>();
    
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
        EmotionStrengths.Add(Emotions.Happy, new[] {0, 1, 2, 3});
        EmotionStrengths.Add(Emotions.Sad, new[] {1, 2, 3, 4});
        EmotionStrengths.Add(Emotions.Angry, new[] {2, 3, 4, 0});
        EmotionStrengths.Add(Emotions.Proud, new[] {3, 4, 0, 1});
        EmotionStrengths.Add(Emotions.Afraid, new[] {4, 0, 1, 2});
    }
}
