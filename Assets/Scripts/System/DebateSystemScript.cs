using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum DebateState {Start, Player, Opponent, Won, Lost}
public class DebateSystemScript : MonoBehaviour
{
    [SerializeField, Header("The Player game object in the scene")] private GameObject player;
    [SerializeField, Header("The point where the enemy is placed")] private Transform opponentSpawn;
    [SerializeField] private GameObject opponentPrefab;
    [SerializeField] private Text notifyText;
    
    public DebateHUDScript playerHUD;
    public DebateHUDScript opponentHUD;
    [SerializeField] private bool _playerHadTurn = false;
    public bool PlayerHadTurn
    {
        get => _playerHadTurn;
        set => _playerHadTurn = value;
    }
    [HideInInspector] public GameObject opponentGO;
    
    private DebateValuesScript _playerValues;
    private DebateValuesScript _opponentValues;
    public DebateState state = DebateState.Start;
    private IEnumerator _debateSetup;
    
    void Start(){
        _playerValues = player.GetComponent<DebateValuesScript>();
        
        opponentGO = Instantiate(opponentPrefab, opponentSpawn);
        _opponentValues = opponentGO.GetComponent<DebateValuesScript>();
        
        playerHUD.SetHUD(_playerValues);
        opponentHUD.SetHUD(_opponentValues);
        state = DebateState.Player;
        StartCoroutine(PlayerTurn());
    }

    /// <summary>
    /// Co-routine for the player's turn
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerTurn(){
        //Waits for Boolean to be set by PLayer Actions script
        while (PlayerHadTurn == false)
        {
            yield return null;
        }
        if(_opponentValues.currentES <= 0)
        {
            state = DebateState.Won;
            yield return new WaitForSeconds(2f);
            StartCoroutine(EndDebate());
        }else
        {
            state = DebateState.Opponent;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(OpponentTurn());
        }
    }
    
    IEnumerator OpponentTurn()
    {
        _playerValues.currentES -= _opponentValues.debaterDamage;
        notifyText.text = $"{_opponentValues.debaterName} dealt {_opponentValues.debaterName} points of emotional strain to you";
        playerHUD.SetES(_playerValues.currentES);
        if(_playerValues.currentES <= 0)
        {
            state = DebateState.Lost;
            StartCoroutine(EndDebate());
        }else
        {
            state = DebateState.Player;
            PlayerHadTurn = false;
            StartCoroutine(PlayerTurn());
        }
        return null;
    }
    
    IEnumerator EndDebate(){
        if(state == DebateState.Won){
            notifyText.text = "You won the debate!";
        }
        else if (state == DebateState.Lost)
        {
            _playerValues.currentES = _playerValues.maxES;
            PlayerPrefs.SetInt("playerES", _playerValues.maxES);
            notifyText.text = "You lost the debate!";
        } 
        else
        {
            Debug.LogError("This function should not have been run in the current state.");
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync("Overworld");
    }
}
