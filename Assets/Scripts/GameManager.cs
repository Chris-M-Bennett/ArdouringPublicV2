using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameObject debateOpponent;
    public static int opponentID;
    public static bool wasBoss = false;
    public static OpponentOverworldStatuses areaStatuses;
    public static bool newGame = true;
    public static bool movedArea;
    public static bool tutorials = true;
    public static bool loadFromSave;
    public static bool healed;
    public static SaveState saveState;
    public static bool loadFromSave;
    public static bool healed;
    public static SaveState saveState;
    public static Sprite debateBg;
    public static string overworld;
    
    public static int happyEnterCount;
    public static int happyDeathCount;
    public static int sadEnterCount;
    public static int sadDeathCount;
    public static int angryEnterCount;
    public static int angryDeathCount;
    public static int proudEnterCount;
    public static int proudDeathCount;
    public static int defeatingCount;
    
    public static List<string[]> debateTimes = new List<string[]>();

    public static Dictionary<Emotions, int[]> emotionStrengths = new Dictionary<Emotions, int[]>();
    
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
        emotionStrengths.Add(Emotions.Happy, new[] {0, 1, 2, 3});
        emotionStrengths.Add(Emotions.Sad, new[] {1, 2, 3, 4});
        emotionStrengths.Add(Emotions.Angry, new[] {2, 3, 4, 0});
        emotionStrengths.Add(Emotions.Proud, new[] {3, 4, 0, 1});
        emotionStrengths.Add(Emotions.Afraid, new[] {4, 0, 1, 2});
        // if (loadFromSave)
        // {
        //     var _overworldPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerOverworldControls>();
        //     _overworldPlayer.transform.position = new Vector2(saveState.playerPos.X,saveState.playerPos.Y);
        //     loadFromSave = false;
        // }
    }
}
