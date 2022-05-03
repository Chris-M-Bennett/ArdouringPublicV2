using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GreyscaleManagerScript : MonoBehaviour
{
    [SerializeField] private OpponentSpawnScript areaBoss;
    private SpriteRenderer[] _backgroundSprites;
    TilemapRenderer[] _tileSprites;

    void Awake(){
        var greyScale = (Material)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Greyscale Sprite Material.mat", typeof(Material));
        var areaStatus = areaBoss.areaStatuses;
        _backgroundSprites = GetComponentsInChildren<SpriteRenderer>();
        _tileSprites = GetComponentsInChildren<TilemapRenderer>();
        if (areaStatus.statuses[areaBoss.id] == 0){
            foreach (var spriteRen in _backgroundSprites)
            {
                spriteRen.material = greyScale;
            }
        
            foreach (var tileRen in _tileSprites)
            {
                tileRen.material = greyScale;
            }
        }
    }
}
