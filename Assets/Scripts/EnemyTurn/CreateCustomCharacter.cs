using System;
using EnemyTurn;
using UnityEditor;
using UnityEngine;

public class CreateCustomCharacter : ScriptableWizard
{
    public string nickname = "default";
    public int maxEnemyES = 100;
    public char currentEmotion = 'h';
    public string patternString = "~.~~...~.~~~~.~.~.~.";
    /*
    public int rows = 4;
    
    public char[,] attackPattern = new char[4, 5]
    {
        {'~', '.', '~', '~', '.'},
        {'.', '.', '~', '.', '~'},
        {'~', '~', '.', '.', '~'},
        {'.', '~', '.', '~', '.'}
    };
    */

    [MenuItem("/My Tools/Create NPC")]
    public static void CreateWizard()
    {
        DisplayWizard<CreateCustomCharacter>("Create Custom NPC", "Add New","Update Selected");
    }

    public void OnWizardCreate()
    {
        GameObject enemyGO = new GameObject();
        enemyGO.name = "Opponent";
        EnemyController foe = enemyGO.AddComponent<EnemyController>();
        foe.maxEnemyES = maxEnemyES;
        foe.nickname = nickname;
        foe.currentEmotion = currentEmotion;
        //   foe.patternString = patternString;
        /*
        foe.rows = rows;
        foe.attackPattern[rows,5] = attackPattern[rows,5];
        for (int i = 0; i < attackPattern.GetLength(0); i++)
        {
            for (int j = 0; j < attackPattern.GetLength(1); j++)
            {
                attackPattern[i, j] = foe.attackPattern[i,j]; //need to figure out how to populate a 2d array with the editor wizard
            }
        }
        */
    }

    private void OnWizardOtherButton()
    {
        if (Selection.activeTransform != null)
        {
            EnemyController opponent = Selection.activeTransform.GetComponent<EnemyController>();
            if (opponent != null)
            {
                opponent.maxEnemyES = maxEnemyES;
                opponent.nickname = nickname;
                opponent.currentEmotion = currentEmotion;
                //   opponent.patternString = patternString;
                /*
                opponent.rows = rows;
                opponent.attackPattern[rows, 5] = attackPattern[rows,5];
                for (int i = 0; i < attackPattern.GetLength(0); i++)
                {
                    for (int j = 0; j < attackPattern.GetLength(1); j++)
                    {
                        attackPattern[i, j] = opponent.attackPattern[i,j]; //need to figure out how to populate a 2d array with the editor wizard
                    }
                }
                */
            }
        }
    }

    public void OnWizardUpdate()
    {
    }
}