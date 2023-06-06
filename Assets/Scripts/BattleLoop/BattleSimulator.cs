using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class BattleSimulator : EditorWindow
{
    public static BattleSystem Instance;
    private List<SupportCharacterSO> _supports = new();
    private List<WeaponSO> _weapons = new();
    private List<Enemy> _enemies = new();
    private DropdownField _playerDropdown; //Used ?

    // SUPPORTS
    private GroupBox _firstSupportGroupBox;
    private GroupBox _secondSupportGroupBox;

    private DropdownField _firstSupportDropdown;
    private DropdownField _secondSupportDropdown;
    private Foldout _firstSupportFoldout;
    private Foldout _secondSupportFoldout;
    private readonly List<TextField> _firstSupportSkillField = new();
    private readonly List<TextField> _secondSupportSkillField = new();

    // WEAPON
    private GroupBox _weaponGroupBox;
    private DropdownField _weaponDropdown;
    private Foldout _weaponFoldout;
    private readonly List<IntegerField> _weaponStatField = new();
    private readonly List<TextField> _weaponSkillField = new();





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

    private readonly List<IntegerField> _firstEnemyStatsField = new();
    private readonly List<IntegerField> _secondEnemyStatsField = new();
    private readonly List<IntegerField> _thirdEnemyStatsField = new();
    private readonly List<IntegerField> _fourthEnemyStatsField = new();
    private readonly List<IntegerField> _fifthEnemyStatsField = new();
    private readonly List<IntegerField> _sixthEnemyStatsField = new();

    private Button _simulateButton; //Used ?

    //TODO: -> Optimize stored datas

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

        _firstSupportGroupBox = root.Q<GroupBox>("FirstSupport");
        _secondSupportGroupBox = root.Q<GroupBox>("SecondSupport");

        _firstSupportSkillField.Add(_firstSupportGroupBox.Q<TextField>("Skill1"));
        _firstSupportSkillField.Add(_firstSupportGroupBox.Q<TextField>("Skill2"));

        _secondSupportSkillField.Add(_secondSupportGroupBox.Q<TextField>("Skill1"));
        _secondSupportSkillField.Add(_secondSupportGroupBox.Q<TextField>("Skill2"));


        _firstSupportDropdown = root.Q<DropdownField>("first-support-dropdown");
        _secondSupportDropdown = root.Q<DropdownField>("second-support-dropdown");

        _firstSupportFoldout = root.Q<Foldout>("first-support-foldout");
        _secondSupportFoldout = root.Q<Foldout>("second-support-foldout");

        _firstSupportFoldout.text = "Support Skills";
        _secondSupportFoldout.text = "Support Skills";


        _weaponGroupBox = root.Q<GroupBox>("Weapon");

        _weaponStatField.Add(_weaponGroupBox.Q<IntegerField>("MainStat"));
        _weaponSkillField.Add(_weaponGroupBox.Q<TextField>("Skill1"));
        _weaponSkillField.Add(_weaponGroupBox.Q<TextField>("Skill2"));

        _weaponDropdown = root.Q<DropdownField>("weapon-dropdown");

        _weaponFoldout = root.Q<Foldout>("weapon-foldout");

        _weaponFoldout.text = "Weapon Skills";


        _firstEnemyGroupBox = root.Q<GroupBox>("FirstEnemy");
        _secondEnemyGroupBox = root.Q<GroupBox>("SecondEnemy");
        _thirdEnemyGroupBox = root.Q<GroupBox>("ThirdEnemy");
        _fourthEnemyGroupBox = root.Q<GroupBox>("FourthEnemy");
        _fifthEnemyGroupBox = root.Q<GroupBox>("FifthEnemy");
        _sixthEnemyGroupBox = root.Q<GroupBox>("SixthEnemy");

        // get the stats fields for all the enemies
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("MaxHp"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("Atk"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("PhysAtk"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("PhysDef"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("MagicAtk"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("MagicDef"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("CritRate"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("CritDamage"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("Speed"));

        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("MaxHp"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("Atk"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("PhysAtk"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("PhysDef"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("MagicAtk"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("MagicDef"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("CritRate"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("CritDamage"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("Speed"));

        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("MaxHp"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("Atk"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("PhysAtk"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("PhysDef"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("MagicAtk"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("MagicDef"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("CritRate"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("CritDamage"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("Speed"));

        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("MaxHp"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("Atk"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("PhysAtk"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("PhysDef"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("MagicAtk"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("MagicDef"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("CritRate"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("CritDamage"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("Speed"));

        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("MaxHp"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("Atk"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("PhysAtk"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("PhysDef"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("MagicAtk"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("MagicDef"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("CritRate"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("CritDamage"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("Speed"));

        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("MaxHp"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("Atk"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("PhysAtk"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("PhysDef"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("MagicAtk"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("MagicDef"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("CritRate"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("CritDamage"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("Speed"));

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

        _supports = new List<SupportCharacterSO>(Resources.LoadAll<SupportCharacterSO>("SO/SupportsCharacter"));

        foreach (SupportCharacterSO support in _supports)
        {
            _firstSupportDropdown.choices.Add("No Support");
            _firstSupportDropdown.choices.Add(support.Name);
            _secondSupportDropdown.choices.Add("No Support");
            _secondSupportDropdown.choices.Add(support.Name);
        }

        _weapons = new List<WeaponSO>(Resources.LoadAll<WeaponSO>("SO/Weapons"));
        foreach (WeaponSO weapon in _weapons)
        {
            _weaponDropdown.choices.Add("No Weapon");
            _weaponDropdown.choices.Add(weapon.Name);
        }


        ChangeFoldoutOnDropdown();
    }

    private void ChangeFoldoutOnDropdown()
    {

        // WEAPON
        _weaponDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Weapon")
            {
                _weaponFoldout.visible = true;
                _weaponFoldout.text = evt.newValue;

                WeaponSO weapon = _weapons.Find(x => x.Name == evt.newValue);

                ChangeFieldsOfWeapon(evt.newValue, _weaponSkillField, _weaponStatField);
            }
            else
            {
                _weaponFoldout.visible = false;
            }
        });

        // SUPPORTSg
        _firstSupportDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Support")
            {
                _firstSupportFoldout.visible = true;
                _firstSupportFoldout.text = evt.newValue;

                ChangeFieldsOfSupport(evt.newValue, _firstSupportSkillField);
            }
            else
            {
                _firstSupportFoldout.visible = false;
            }
        });

        _secondSupportDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Support")
            {
                _secondSupportFoldout.visible = true;
                _secondSupportFoldout.text = evt.newValue;

                ChangeFieldsOfSupport(evt.newValue, _secondSupportSkillField);
            }
            else
            {
                _secondSupportFoldout.visible = false;
            }
        });

        // ENEMIES

        _firstEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _firstEnemyFoldout.visible = true;
                _firstEnemyFoldout.text = evt.newValue;

                ChangeFieldsOfEnemy(evt.newValue, _firstEnemyStatsField);
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
                _secondEnemyFoldout.visible = true;
                _secondEnemyFoldout.text = evt.newValue;

                ChangeFieldsOfEnemy(evt.newValue, _secondEnemyStatsField);

            }
            else
            {
                _secondEnemyFoldout.visible = false;
            }
        });
        _thirdEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _thirdEnemyFoldout.visible = true;
                _thirdEnemyFoldout.text = evt.newValue;

                ChangeFieldsOfEnemy(evt.newValue, _thirdEnemyStatsField);
            }
            else
            {
                _thirdEnemyFoldout.visible = false;
            }
        });
        _fourthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _fourthEnemyFoldout.visible = true;
                _fourthEnemyFoldout.text = evt.newValue;

                ChangeFieldsOfEnemy(evt.newValue, _fourthEnemyStatsField);
            }
            else
            {
                _fourthEnemyFoldout.visible = false;
            }
        });
        _fifthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _fifthEnemyFoldout.visible = true;
                _fifthEnemyFoldout.text = evt.newValue;

                ChangeFieldsOfEnemy(evt.newValue, _fifthEnemyStatsField);
            }
            else
            {
                _fifthEnemyFoldout.visible = false;
            }
        });
        _sixthEnemyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Enemy")
            {
                _sixthEnemyFoldout.visible = true;
                _sixthEnemyFoldout.text = evt.newValue;

                ChangeFieldsOfEnemy(evt.newValue, _sixthEnemyStatsField);
            }
            else
            {
                _sixthEnemyFoldout.visible = false;
            }
        });
    }

    private void ChangeFieldsOfEnemy(string enemyName, List<IntegerField> enemyStatsField)
    {
        Enemy enemy = _enemies.Find(x => x.Name == enemyName);

        foreach (var item in enemy.GetAllStats())
        {
            foreach (var field in enemyStatsField)
            {
                if (field.name == item.Key)
                {
                    field.SetValueWithoutNotify(item.Value);
                }
            }
        }
    }

    private void ChangeFieldsOfSupport(string supportName, List<TextField> supportFields)
    {
        SupportCharacterSO support = _supports.Find(x => x.Name == supportName);



        // FIXME: -> The skills are not assigned to the support
        // supportFields[0].SetValueWithoutNotify(support.PrimarySkill.Data.Name);
        // supportFields[1].SetValueWithoutNotify(support.SecondarySkill.Data.Name);



    }

    private void ChangeFieldsOfWeapon(string weaponName, List<TextField> weaponSkillFields, List<IntegerField> weaponStatFields)
    {
        WeaponSO weapon = _weapons.Find(x => x.Name == weaponName);

        // Change the name of the stat field to the name of the stat
        weaponStatFields[0].label = weapon.Stat.ToString();
        weaponStatFields[0].SetValueWithoutNotify((int)weapon.StatValue);

        // FIXME: -> The skills are not assigned to the weapon
        // weaponSkillFields[0].SetValueWithoutNotify(weapon.FirstSkillData.Name);
        // weaponSkillFields[1].SetValueWithoutNotify(weapon.SecondSkillData.Name);



    }

    private void OnInspectorUpdate()
    {
        this.Repaint();
    }
}