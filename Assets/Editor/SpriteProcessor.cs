using System;
using UnityEditor;
using UnityEngine;

public class SpriteProcessor : AssetPostprocessor 
{
    private void OnPostprocessTexture(Texture2D texture)
    {
        string texAssetPath = assetPath.ToLower();
        bool isWantedFolder = texAssetPath.IndexOf("/sprite/") != -1; // check if path is a Sprite folder
        if (isWantedFolder)
        {
            TextureImporter textureImporter = assetImporter as TextureImporter; // change asset property
            textureImporter.textureType = TextureImporterType.Sprite;
        }
    }
}
