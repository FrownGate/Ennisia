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

    // create a dictionary of stats for the enemies
    private Dictionary<string, int> _firstEnemyStats = new();



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

        window.position = new Rect(Screen.width / 2, Screen.height / 2, 1000, 1000);
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
        _allies.Add("No Enemy");
        for (int i = 0; i < 10; i++)
        {
            _allies.Add($"Enemy {i + 1}");
        }

        // init numbers for the stats of the first enemy
        _firstEnemyStats.Add("HP", 100);
        _firstEnemyStats.Add("MP", 10);
        _firstEnemyStats.Add("ATK", 20);
        _firstEnemyStats.Add("DEF", 0);
        _firstEnemyStats.Add("MAG", 0);
        _firstEnemyStats.Add("RES", 0);
        _firstEnemyStats.Add("SPD", 0);


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
        _fifthEnemyFoldout = root.Q<Foldout>("fifth-enemy-foldout");
        _sixthEnemyFoldout = root.Q<Foldout>("sixth-enemy-foldout");


        // TODO: add enemies to dropdown
        _firstEnemyDropdown.choices = new List<string>(_allies);
        // _firstEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _secondEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _thirdEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _fourthEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _fifthEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));
        // _sixthEnemyDropdown.choices = new List<string>(Enum.GetNames(typeof(EnemyType)));

        // set foldouts to the same text
        _firstEnemyFoldout.text = "Enemy Stats";

        _secondEnemyFoldout.text = "Enemy Stats";
        _thirdEnemyFoldout.text = "Enemy Stats";
        _fourthEnemyFoldout.text = "Enemy Stats";
        _fifthEnemyFoldout.text = "Enemy Stats";
        _sixthEnemyFoldout.text = "Enemy Stats";



    }

    public void OnGUI()
    {

        // if the dropdown is changed, change the foldout text
        ChangeFoldoutOnDropdown();


    }

    private void ChangeFoldoutOnDropdown()
    {
        _firstEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;
                // create input fields for the stats of the enemy
                foreach (KeyValuePair<string, int> stat in _firstEnemyStats)
                {
                    // create a label for the stat





                }
            }
            else
            {
                _firstEnemyFoldout.visible = false;
            }
        });
        _secondEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            Debug.Log("coucou");
            _secondEnemyFoldout.text = evt.newValue;
        });
        _thirdEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            _thirdEnemyFoldout.text = evt.newValue;
        });
        _fourthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            _fourthEnemyFoldout.text = evt.newValue;
        });
        _fifthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            _fifthEnemyFoldout.text = evt.newValue;
        });
        _sixthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            _sixthEnemyFoldout.text = evt.newValue;
        });


    }

    private void OnInspectorUpdate()
    {
        // EditorGUILayout.Foldout(_firstEnemyFoldout.visible, "First Enemy Foldout");
        this.Repaint();



    }




}