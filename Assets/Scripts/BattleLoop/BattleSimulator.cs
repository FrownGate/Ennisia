using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;


public class BattleSimulator : EditorWindow
{

    public static BattleSystem Instance;
    private List<string> _allies = new();
    private List<Enemy> _enemies = new();
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

    private List<IntegerField> _firstEnemyStatsField = new();
    private List<IntegerField> _secondEnemyStatsField = new();
    private List<IntegerField> _thirdEnemyStatsField = new();
    private List<IntegerField> _fourthEnemyStatsField = new();
    private List<IntegerField> _fifthEnemyStatsField = new();
    private List<IntegerField> _sixthEnemyStatsField = new();




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





        _firstEnemyGroupBox = root.Q<GroupBox>("FirstEnemy");
        _secondEnemyGroupBox = root.Q<GroupBox>("SecondEnemy");
        _thirdEnemyGroupBox = root.Q<GroupBox>("ThirdEnemy");
        _fourthEnemyGroupBox = root.Q<GroupBox>("FourthEnemy");
        _fifthEnemyGroupBox = root.Q<GroupBox>("FifthEnemy");
        _sixthEnemyGroupBox = root.Q<GroupBox>("SixthEnemy");


        // get the stats fields for all the enemies
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-hp"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-atk"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-physatk"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-physdef"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-magicatk"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-magicdef"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-critrate"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-critdamage"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("first-enemy-spd"));


        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-hp"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-atk"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-physatk"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-physdef"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-magicatk"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-magicdef"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-critrate"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-critdamage"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("second-enemy-spd"));

        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-hp"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-atk"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-physatk"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-physdef"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-magicatk"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-magicdef"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-critrate"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-critdamage"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("third-enemy-spd"));

        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-hp"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-atk"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-physatk"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-physdef"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-magicatk"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-magicdef"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-critrate"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-critdamage"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("fourth-enemy-spd"));

        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-hp"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-atk"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-physatk"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-physdef"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-magicatk"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-magicdef"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-critrate"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-critdamage"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("fifth-enemy-spd"));

        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-hp"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-atk"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-physatk"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-physdef"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-magicatk"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-magicdef"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-critrate"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-critdamage"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("sixth-enemy-spd"));






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



        // Set Base value for the foldouts
        _firstEnemyFoldout.text = "Enemy Stats";
        _secondEnemyFoldout.text = "Enemy Stats";
        _thirdEnemyFoldout.text = "Enemy Stats";
        _fourthEnemyFoldout.text = "Enemy Stats";
        _fifthEnemyFoldout.text = "Enemy Stats";
        _sixthEnemyFoldout.text = "Enemy Stats";



    }

    public void OnGUI()
    {
        EnemyLoader enemyLoader = new();
        _enemies = enemyLoader.LoadEnemies("Assets/Resources/CSV/Enemies.csv");

        // TODO: add enemies to dropdown
        foreach (Enemy enemy in _enemies)
        {
            _firstEnemyDropdown.choices.Add("No Enemy");
            _firstEnemyDropdown.choices.Add(enemy.Name);
            _secondEnemyDropdown.choices.Add("No Enemy");
            _secondEnemyDropdown.choices.Add(enemy.Name);
            _thirdEnemyDropdown.choices.Add("No Enemy");
            _thirdEnemyDropdown.choices.Add(enemy.Name);
            _fourthEnemyDropdown.choices.Add("No Enemy");
            _fourthEnemyDropdown.choices.Add(enemy.Name);
            _fifthEnemyDropdown.choices.Add("No Enemy");
            _fifthEnemyDropdown.choices.Add(enemy.Name);
            _sixthEnemyDropdown.choices.Add("No Enemy");
            _sixthEnemyDropdown.choices.Add(enemy.Name);


        }

        // if the dropdown is changed, change the foldout text
        ChangeFoldoutOnDropdown();
        // BattleSystem.Instance.SimulateBattle();

        // Set the first enemy stats



    }

    private void ChangeFoldoutOnDropdown()
    {
        _firstEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;

                ChangeStatsOfEnemy(evt.newValue, _firstEnemyStatsField);

            }
            else
            {
                _firstEnemyFoldout.visible = false;
            }
        });
        _secondEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;

                ChangeStatsOfEnemy(evt.newValue, _firstEnemyStatsField);

            }
            else
            {
                _firstEnemyFoldout.visible = false;
            }
        });
        _thirdEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;

                ChangeStatsOfEnemy(evt.newValue, _firstEnemyStatsField);

            }
            else
            {
                _firstEnemyFoldout.visible = false;
            }
        });
        _fourthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;

                ChangeStatsOfEnemy(evt.newValue, _firstEnemyStatsField);

            }
            else
            {
                _firstEnemyFoldout.visible = false;
            }
        });
        _fifthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;

                ChangeStatsOfEnemy(evt.newValue, _firstEnemyStatsField);

            }
            else
            {
                _firstEnemyFoldout.visible = false;
            }
        });
        _sixthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;

                ChangeStatsOfEnemy(evt.newValue, _firstEnemyStatsField);

            }
            else
            {
                _firstEnemyFoldout.visible = false;
            }
        });


    }

    private void ChangeStatsOfEnemy(string enemyName, List<IntegerField> enemyStatsField)
    {

        Enemy enemy = _enemies.Find(x => x.Name == enemyName);

        // loop through all the IntegerField of the foldout and set the value to the enemy stats
        for (int i = 0; i < enemyStatsField.Count; i++)
        {
            enemyStatsField[i].value = (int)enemy.GetType().GetField(enemyStatsField[i].name).GetValue(enemy);
        }


    }

    private void OnInspectorUpdate()
    {
        // EditorGUILayout.Foldout(_firstEnemyFoldout.visible, "First Enemy Foldout");
        this.Repaint();



    }

    // create a function to parse the Enemies.csv file and create a list of enemies





}