using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;


public class BattleSimulator : EditorWindow
{
    private List<string> _allies = new();
    private DropdownField _alliesDropdown;

    // ENEMIES
    private GroupBox _firstEnemyGroupBox;
    private GroupBox _secondEnemyGroupBox;
    private GroupBox _thirdEnemyGroupBox;
    private GroupBox _fourthEnemyGroupBox;
    private GroupBox _fifthEnemyGroupBox;
    private GroupBox _sixthEnemyGroupBox;

    private DropdownField _firstEnemyDropdown;
    private DropdownField _secondEnemyDropdown;
    private DropdownField _thirdEnemyDropdown;
    private DropdownField _fourthEnemyDropdown;
    private DropdownField _fifthEnemyDropdown;
    private DropdownField _sixthEnemyDropdown;
    private Foldout _firstEnemyFoldout;
    private Foldout _secondEnemyFoldout;
    private Foldout _thirdEnemyFoldout;
    private Foldout _fourthEnemyFoldout;
    private Foldout _fifthEnemyFoldout;
    private Foldout _sixthEnemyFoldout;


    private Button _simulateButton;

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
        GUIContent icon = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow");
        window.titleContent = new GUIContent("Battle Simulator", icon.image, "Battle Simulator");

        window.position = new Rect(Screen.width / 2, Screen.height / 2, 700, 700);
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BattleLoop/BattleSimulator.uxml");
        TemplateContainer labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        // StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BattleLoop/BattleSimulator.uss");
        // root.styleSheets.Add(styleSheet);

        // add values in _allies for testing
        for (int i = 0; i < 10; i++)
        {
            _allies.Add($"Ally {i}");
        }

        _firstEnemyGroupBox = root.Q<GroupBox>("FirstEnemy");
        _secondEnemyGroupBox = root.Q<GroupBox>("SecondEnemy");
        _thirdEnemyGroupBox = root.Q<GroupBox>("ThirdEnemy");
        _fourthEnemyGroupBox = root.Q<GroupBox>("FourthEnemy");
        _fifthEnemyGroupBox = root.Q<GroupBox>("FifthEnemy");
        _sixthEnemyGroupBox = root.Q<GroupBox>("SixthEnemy");




        _firstEnemyDropdown = root.Q<DropdownField>("first-enemy-dropdown");
        _secondEnemyDropdown = root.Q<DropdownField>("second-enemy-dropdown");
        _thirdEnemyDropdown = root.Q<DropdownField>("third-enemy-dropdown");
        _fourthEnemyDropdown = root.Q<DropdownField>("fourth-enemy-dropdown");
        _fifthEnemyDropdown = root.Q<DropdownField>("fifth-enemy-dropdown");
        _sixthEnemyDropdown = root.Q<DropdownField>("sixth-enemy-dropdown");

        _firstEnemyFoldout = root.Q<Foldout>("first-enemy-foldout");
        _secondEnemyFoldout = root.Q<Foldout>("second-enemy-foldout");
        _thirdEnemyFoldout = root.Q<Foldout>("third-enemy-foldout");
        _fourthEnemyFoldout = root.Q<Foldout>("fourth-enemy-foldout");
        _fifthEnemyDropdown = root.Q<DropdownField>("fifth-enemy-dropdown");
        _sixthEnemyDropdown = root.Q<DropdownField>("sixth-enemy-dropdown");


        // TODO: add enemies to dropdown
        _firstEnemyDropdown.choices = new List<string>(_allies);
        // _firstEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _secondEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _thirdEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _fourthEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _fifthEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _sixthEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));



    }


    // private void OnGUI()
    // {
    //     // create random enemies for testing
    //     for (int i = 0; i < 10; i++)
    //     {
    //         _enemies.Add($"Enemy {i}");
    //     }
    //     rootVisualElement.Clear();
    //     VisualElement main = rootVisualElement;


    //     float splitPosition = 0.5f; // Split position as a normalized value (0 to 1)

    //     float splitPixelPosition = splitPosition * Screen.width;

    //     // LEFT PANEL
    //     GUI.BeginGroup(new Rect(0, 0, splitPixelPosition, Screen.height));
    //     GUI.Box(new Rect(0, 0, splitPixelPosition, Screen.height), "");
    //     // Add left panel contents here

    //     AlliesGUI();

    //     // simulateButton.clicked += () => { Debug.Log("Simulate"); };
    //     GUI.EndGroup();

    //     // RIGHT PANEL
    //     GUI.BeginGroup(new Rect(splitPixelPosition, 0, Screen.width - splitPixelPosition, Screen.height));
    //     GUI.Box(new Rect(0, 0, Screen.width - splitPixelPosition, Screen.height), "");
    //     // Add right panel contents here

    //     EnemiesGUI();

    //     UpdateEnemySelectionGUI();
    //     GUI.EndGroup();


    // }

    // private void AlliesGUI()
    // {
    //     // EditorGUILayout.BeginHorizontal();
    //     EditorGUILayout.BeginVertical();

    //     // create guilayout label centered
    //     GUILayout.Label("Allies",
    //        EditorStyles.boldLabel,
    //        GUILayout.Width(200),
    //        GUILayout.Height(30),
    //        GUILayout.ExpandWidth(true),
    //        GUILayout.ExpandHeight(false)
    //        );

    //     // EditorGUILayout.EndHorizontal();
    //     EditorGUILayout.EndVertical();


    // }

    // private void EnemiesGUI()
    // {
    //     // EditorGUILayout.BeginHorizontal();
    //     EditorGUILayout.BeginVertical();
    //     GUILayout.Label("Enemies",
    //        EditorStyles.boldLabel,
    //        GUILayout.Width(200),
    //        GUILayout.Height(30),
    //        GUILayout.ExpandWidth(true),
    //        GUILayout.ExpandHeight(false)
    //        );
    //     // EditorGUILayout.EndHorizontal();
    //     EditorGUILayout.EndVertical();

    // }

    // private void UpdateEnemySelectionGUI()
    // {

    //     EditorGUILayout.BeginVertical(EditorStyles.helpBox);


    //     EditorGUILayout.LabelField("Enemies", EditorStyles.boldLabel);

    //     // foreach (string enemy in _enemies)
    //     // {
    //     //     Button enemyButton = new()
    //     //     {
    //     //         text = enemy,
    //     //         style =
    //     //         {
    //     //             width = 100,
    //     //             height = 30,
    //     //             marginBottom = 30,
    //     //             marginTop = 15,
    //     //             paddingLeft = 5,
    //     //             paddingRight = 5,
    //     //             paddingTop = 5,
    //     //             paddingBottom = 5,
    //     //             alignItems = Align.Center,
    //     //             justifyContent = Justify.Center,
    //     //         },
    //     //     };
    //     //     _rightPane.Add(enemyButton);
    //     // }


    //     EditorGUILayout.EndVertical();

    // }

}