using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Opponents;
using Player;
using UnityEngine;
using Newtonsoft.Json;
public class SavePoint : MonoBehaviour
{
    private static DebateValuesScript _playerValues;
    public bool healed;
    private void Start()
    {
        _playerValues = GameObject.FindWithTag("Player").GetComponent<PlayerDebateValues>();
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
        if (Input.GetKeyDown(KeyCode.P)) //save code goes here
        {
            File.WriteAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json", JsonConvert.SerializeObject(healed));
        }
        if (Input.GetKeyDown(KeyCode.L)) //load save file code goes here
        {
            healed = JsonConvert.DeserializeObject<bool>(File.ReadAllText(@"c:\Users\Jake\Desktop\TestSaveFolder\saveTest.json"));
        }
    }
}
