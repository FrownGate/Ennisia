using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;


public class BattleSimulator : EditorWindow
{

    private List<string> _enemies = new();
    [MenuItem("Tools/Battle Simulator")]
    public static void ShowWindow()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogError("You must be in play mode to use the Battle Simulator");
            return;
        }
        GetWindow<BattleSimulator>("Battle Simulator");
        BattleSimulator window = GetWindow<BattleSimulator>("Battle Simulator");
        window.titleContent = new GUIContent("Battle Simulator");
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 700, 700);
    }


    private void OnGUI()
    {
        // create random enemies for testing
        for (int i = 0; i < 10; i++)
        {
            _enemies.Add($"Enemy {i}");
        }
        rootVisualElement.Clear();
        VisualElement main = rootVisualElement;


        float splitPosition = 0.5f; // Split position as a normalized value (0 to 1)

        float splitPixelPosition = splitPosition * Screen.width;

        // LEFT PANEL
        GUI.BeginGroup(new Rect(0, 0, splitPixelPosition, Screen.height));
        GUI.Box(new Rect(0, 0, splitPixelPosition, Screen.height), "");
        // Add left panel contents here

        AlliesGUI();

        // simulateButton.clicked += () => { Debug.Log("Simulate"); };
        GUI.EndGroup();

        // RIGHT PANEL
        GUI.BeginGroup(new Rect(splitPixelPosition, 0, Screen.width - splitPixelPosition, Screen.height));
        GUI.Box(new Rect(0, 0, Screen.width - splitPixelPosition, Screen.height), "");
        // Add right panel contents here

        EnemiesGUI();

        UpdateEnemySelectionGUI();
        GUI.EndGroup();


    }

    private void AlliesGUI()
    {
        // EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        // create guilayout label centered
        GUILayout.Label("Allies",
           EditorStyles.boldLabel,
           GUILayout.Width(200),
           GUILayout.Height(30),
           GUILayout.ExpandWidth(true),
           GUILayout.ExpandHeight(false)
           );

        // EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();


    }

    private void EnemiesGUI()
    {
        // EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Enemies",
           EditorStyles.boldLabel,
           GUILayout.Width(200),
           GUILayout.Height(30),
           GUILayout.ExpandWidth(true),
           GUILayout.ExpandHeight(false)
           );
        // EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

    }

    private void UpdateEnemySelectionGUI()
    {

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);


        EditorGUILayout.LabelField("Enemies", EditorStyles.boldLabel);

        // foreach (string enemy in _enemies)
        // {
        //     Button enemyButton = new()
        //     {
        //         text = enemy,
        //         style =
        //         {
        //             width = 100,
        //             height = 30,
        //             marginBottom = 30,
        //             marginTop = 15,
        //             paddingLeft = 5,
        //             paddingRight = 5,
        //             paddingTop = 5,
        //             paddingBottom = 5,
        //             alignItems = Align.Center,
        //             justifyContent = Justify.Center,
        //         },
        //     };
        //     _rightPane.Add(enemyButton);
        // }


        EditorGUILayout.EndVertical();

    }

}