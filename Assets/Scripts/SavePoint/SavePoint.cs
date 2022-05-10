using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Opponents;
using Player;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using static GameManager;

public class SavePoint : MonoBehaviour
{
    private static DebateValuesScript _playerValues;
    private static PlayerOverworldControls _overworldPlayer;
    //private Vector2 playerPos;
    public bool healed, loaded;
    //private static GameManager _gameManager;

    private void Start()
    {
        //_gameManager = GetComponent<GameManager>();
        _playerValues = GameObject.FindWithTag("Player").GetComponent<PlayerDebateValues>();
        _overworldPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerOverworldControls>();
        healed = false;
    }
    /*
    private void OnCollisionEnter2D(Collision2D touch)
    {
        if (touch.gameObject.tag == "Player")
        {
            healed = true;
            //_playerValues.currentES = _playerValues.maxES;
            Debug.Log("Save: Player fully healed");
        }
    }
    */
    private void Update()
    {
        // if (loaded)
        // {
        //     _overworldPlayer.transform.position = new Vector2(playerPos.X,playerPos.Y);
        //     loaded = false;
        // }
        if (Input.GetKeyDown(KeyCode.P))// && _gameManager != null) //save code goes here
        {
            var playerPos = new Coords(_overworldPlayer.transform.position.x,_overworldPlayer.transform.position.y);
            var tempSaveState = new SaveState();
            tempSaveState.playerPos = playerPos;
            //File.WriteAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json", JsonConvert.SerializeObject(healed));
            File.WriteAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json", JsonConvert.SerializeObject(tempSaveState));

            //File.WriteAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json", JsonConvert.SerializeObject(areaStatuses));

        }
        if (Input.GetKeyDown(KeyCode.L)) //load save file code goes here
        {
            //areaStatuses = JsonConvert.DeserializeObject<OpponentOverworldStatuses>(File.ReadAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json"));
            saveState = JsonConvert.DeserializeObject<SaveState>(File.ReadAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json"));
            //loadFromSave = true;
            var playerPos = saveState.playerPos;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            _overworldPlayer.transform.position = new Vector2(playerPos.X,playerPos.Y);
            //areaStatuses = JsonConvert.DeserializeObject<OpponentOverworldStatuses>(File.ReadAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json"));

        }    
    }
}
