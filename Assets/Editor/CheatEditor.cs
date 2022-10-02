using System;
using System.Collections;
using System.Collections.Generic;
using CosmosDefender;
using UnityEditor;
using UnityEngine;

public class CheatEditor : EditorWindow
{
#if UNITY_EDITOR

    [MenuItem("CosmosDefender/Cheats")]
    private static void Open()
    {
        GetWindow<CheatEditor>();
    }

    private int moneyToAdd = 0;
    
    public void OnGUI()
    {
        GUILayout.Label("Money Cheats", EditorStyles.boldLabel);


        moneyToAdd = EditorGUILayout.IntField("Money Amount", moneyToAdd);
        if (GUILayout.Button("Add Money"))
        {
            if (!Application.isPlaying) return;
            //FindObjectOfType<EconomyConfig>().AddMoney(moneyToAdd);
        }
    }
#endif
}
