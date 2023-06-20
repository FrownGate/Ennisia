using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class BattleSimulator : EditorWindow
{
    //TODO: -> Optimize stored datas with enum, arrays, loops, etc.

    public static BattleSystem Instance;
    private List<SupportCharacterSO> _supports = new();
    private List<GearSO> _weapons = new();
    // TODO: To Enemy 
    private List<Entity> _enemies = new();
    private List<GearSO> _gears = new();
    // TODO: To Enemy 
    private List<Entity> _selectedEnemies = new();
    private List<SupportCharacterSO> _selectedSupports = new();
    private List<GearSO> _selectedGears = new();
    private GearSO _selectedWeapon;

    List<EnemyInfo> _enemiesInfo = new();

    // GEARS
    private GroupBox _helmetGroupBox;
    private GroupBox _chestGroupBox;
    private GroupBox _bootsGroupBox;
    private GroupBox _ringGroupBox;
    private GroupBox _necklaceGroupBox;
    private GroupBox _earringsGroupBox;
    private DropdownField _helmetDropdown;
    private DropdownField _chestDropdown;
    private DropdownField _bootsDropdown;
    private DropdownField _ringDropdown;
    private DropdownField _necklaceDropdown;
    private DropdownField _earringsDropdown;
    private Foldout _helmetFoldout;
    private Foldout _chestFoldout;
    private Foldout _bootsFoldout;
    private Foldout _ringFoldout;
    private Foldout _necklaceFoldout;
    private Foldout _earringsFoldout;
    private readonly List<IntegerField> _helmetStatField = new();
    private readonly List<IntegerField> _chestStatField = new();
    private readonly List<IntegerField> _bootsStatField = new();
    private readonly List<IntegerField> _ringStatField = new();
    private readonly List<IntegerField> _necklaceStatField = new();
    private readonly List<IntegerField> _earringsStatField = new();


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
    private readonly List<DropdownField> _enemiesDropdown = new();
    private Foldout _firstEnemyFoldout;
    private Foldout _secondEnemyFoldout;
    private Foldout _thirdEnemyFoldout;
    private Foldout _fourthEnemyFoldout;
    private Foldout _fifthEnemyFoldout;
    private Foldout _sixthEnemyFoldout;
    private readonly List<Foldout> _enemiesFoldout = new();

    private readonly List<IntegerField> _firstEnemyStatsField = new();
    private readonly List<IntegerField> _secondEnemyStatsField = new();
    private readonly List<IntegerField> _thirdEnemyStatsField = new();
    private readonly List<IntegerField> _fourthEnemyStatsField = new();
    private readonly List<IntegerField> _fifthEnemyStatsField = new();
    private readonly List<IntegerField> _sixthEnemyStatsField = new();
    private readonly Dictionary<DropdownField, List<IntegerField>> _firstEnemyInfo = new();
    private readonly Dictionary<DropdownField, List<IntegerField>> _secondEnemyInfo = new();
    private readonly Dictionary<DropdownField, List<IntegerField>> _thirdEnemyInfo = new();
    private readonly Dictionary<DropdownField, List<IntegerField>> _fourthEnemyInfo = new();
    private readonly Dictionary<DropdownField, List<IntegerField>> _fifthEnemyInfo = new();
    private readonly Dictionary<DropdownField, List<IntegerField>> _sixthEnemyInfo = new();
    private readonly List<Dictionary<DropdownField, List<IntegerField>>> _enemiesInfo = new();
    public List<Object> _enemiesStruct = new();
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

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BattleSimulator.uxml");
        TemplateContainer labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        _simulateButton = root.Q<Button>("simulate-button");

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

        _helmetGroupBox = root.Q<GroupBox>("Helmet");
        _chestGroupBox = root.Q<GroupBox>("Chest");
        _bootsGroupBox = root.Q<GroupBox>("Boots");
        _ringGroupBox = root.Q<GroupBox>("Ring");
        _necklaceGroupBox = root.Q<GroupBox>("Necklace");
        _earringsGroupBox = root.Q<GroupBox>("Earrings");

        _helmetStatField.Add(_helmetGroupBox.Q<IntegerField>("Hp"));
        _helmetStatField.Add(_helmetGroupBox.Q<IntegerField>("Substat1"));
        _helmetStatField.Add(_helmetGroupBox.Q<IntegerField>("Substat2"));
        _helmetStatField.Add(_helmetGroupBox.Q<IntegerField>("Substat3"));
        _helmetStatField.Add(_helmetGroupBox.Q<IntegerField>("Substat4"));

        _chestStatField.Add(_chestGroupBox.Q<IntegerField>("MainStat"));
        _chestStatField.Add(_chestGroupBox.Q<IntegerField>("Substat1"));
        _chestStatField.Add(_chestGroupBox.Q<IntegerField>("Substat2"));
        _chestStatField.Add(_chestGroupBox.Q<IntegerField>("Substat3"));
        _chestStatField.Add(_chestGroupBox.Q<IntegerField>("Substat4"));

        _bootsStatField.Add(_bootsGroupBox.Q<IntegerField>("Defense"));
        _bootsStatField.Add(_bootsGroupBox.Q<IntegerField>("Substat1"));
        _bootsStatField.Add(_bootsGroupBox.Q<IntegerField>("Substat2"));
        _bootsStatField.Add(_bootsGroupBox.Q<IntegerField>("Substat3"));
        _bootsStatField.Add(_bootsGroupBox.Q<IntegerField>("Substat4"));

        _ringStatField.Add(_ringGroupBox.Q<IntegerField>("MainStat"));
        _ringStatField.Add(_ringGroupBox.Q<IntegerField>("Substat1"));
        _ringStatField.Add(_ringGroupBox.Q<IntegerField>("Substat2"));
        _ringStatField.Add(_ringGroupBox.Q<IntegerField>("Substat3"));
        _ringStatField.Add(_ringGroupBox.Q<IntegerField>("Substat4"));

        _necklaceStatField.Add(_necklaceGroupBox.Q<IntegerField>("MainStat"));
        _necklaceStatField.Add(_necklaceGroupBox.Q<IntegerField>("Substat1"));
        _necklaceStatField.Add(_necklaceGroupBox.Q<IntegerField>("Substat2"));
        _necklaceStatField.Add(_necklaceGroupBox.Q<IntegerField>("Substat3"));
        _necklaceStatField.Add(_necklaceGroupBox.Q<IntegerField>("Substat4"));

        _earringsStatField.Add(_earringsGroupBox.Q<IntegerField>("MainStat"));
        _earringsStatField.Add(_earringsGroupBox.Q<IntegerField>("Substat1"));
        _earringsStatField.Add(_earringsGroupBox.Q<IntegerField>("Substat2"));
        _earringsStatField.Add(_earringsGroupBox.Q<IntegerField>("Substat3"));
        _earringsStatField.Add(_earringsGroupBox.Q<IntegerField>("Substat4"));

        _helmetDropdown = root.Q<DropdownField>("helmet-dropdown");
        _chestDropdown = root.Q<DropdownField>("chest-dropdown");
        _bootsDropdown = root.Q<DropdownField>("boots-dropdown");
        _ringDropdown = root.Q<DropdownField>("ring-dropdown");
        _necklaceDropdown = root.Q<DropdownField>("necklace-dropdown");
        _earringsDropdown = root.Q<DropdownField>("earrings-dropdown");

        _helmetFoldout = root.Q<Foldout>("helmet-foldout");
        _chestFoldout = root.Q<Foldout>("chest-foldout");
        _bootsFoldout = root.Q<Foldout>("boots-foldout");
        _ringFoldout = root.Q<Foldout>("ring-foldout");
        _necklaceFoldout = root.Q<Foldout>("necklace-foldout");
        _earringsFoldout = root.Q<Foldout>("earrings-foldout");


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
        //TODO -> use enums with list or dictionary to optimize
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("HP"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("Attack"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("PhysicalDamages"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("PhysicalDefense"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("MagicalDamages"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("MagicalDefense"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("CritRate"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("CritDmg"));
        _firstEnemyStatsField.Add(_firstEnemyGroupBox.Q<IntegerField>("Speed"));

        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("HP"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("Attack"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("PhysicalDamages"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("PhysicalDefense"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("MagicalDamages"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("MagicalDefense"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("CritRate"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("CritDmg"));
        _secondEnemyStatsField.Add(_secondEnemyGroupBox.Q<IntegerField>("Speed"));

        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("HP"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("Attack"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("PhysicalDamages"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("PhysicalDefense"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("MagicalDamages"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("MagicalDefense"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("CritRate"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("CritDmg"));
        _thirdEnemyStatsField.Add(_thirdEnemyGroupBox.Q<IntegerField>("Speed"));

        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("HP"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("Attack"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("PhysicalDamages"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("PhysicalDefense"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("MagicalDamages"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("MagicalDefense"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("CritRate"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("CritDmg"));
        _fourthEnemyStatsField.Add(_fourthEnemyGroupBox.Q<IntegerField>("Speed"));

        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("HP"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("Attack"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("PhysicalDamages"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("PhysicalDefense"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("MagicalDamages"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("MagicalDefense"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("CritRate"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("CritDmg"));
        _fifthEnemyStatsField.Add(_fifthEnemyGroupBox.Q<IntegerField>("Speed"));

        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("HP"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("Attack"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("PhysicalDamages"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("PhysicalDefense"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("MagicalDamages"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("MagicalDefense"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("CritRate"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("CritDmg"));
        _sixthEnemyStatsField.Add(_sixthEnemyGroupBox.Q<IntegerField>("Speed"));

        _firstEnemyDropdown = root.Q<DropdownField>("first-enemy-dropdown");
        _secondEnemyDropdown = root.Q<DropdownField>("second-enemy-dropdown");
        _thirdEnemyDropdown = root.Q<DropdownField>("third-enemy-dropdown");
        _fourthEnemyDropdown = root.Q<DropdownField>("fourth-enemy-dropdown");
        _fifthEnemyDropdown = root.Q<DropdownField>("fifth-enemy-dropdown");
        _sixthEnemyDropdown = root.Q<DropdownField>("sixth-enemy-dropdown");

        _enemiesDropdown.Add(_firstEnemyDropdown);
        _enemiesDropdown.Add(_secondEnemyDropdown);
        _enemiesDropdown.Add(_thirdEnemyDropdown);
        _enemiesDropdown.Add(_fourthEnemyDropdown);
        _enemiesDropdown.Add(_fifthEnemyDropdown);
        _enemiesDropdown.Add(_sixthEnemyDropdown);

        _firstEnemyFoldout = root.Q<Foldout>("first-enemy-foldout");
        _secondEnemyFoldout = root.Q<Foldout>("second-enemy-foldout");
        _thirdEnemyFoldout = root.Q<Foldout>("third-enemy-foldout");
        _fourthEnemyFoldout = root.Q<Foldout>("fourth-enemy-foldout");
        _fifthEnemyFoldout = root.Q<Foldout>("fifth-enemy-foldout");
        _sixthEnemyFoldout = root.Q<Foldout>("sixth-enemy-foldout");

        _enemiesFoldout.Add(_firstEnemyFoldout);
        _enemiesFoldout.Add(_secondEnemyFoldout);
        _enemiesFoldout.Add(_thirdEnemyFoldout);
        _enemiesFoldout.Add(_fourthEnemyFoldout);
        _enemiesFoldout.Add(_fifthEnemyFoldout);
        _enemiesFoldout.Add(_sixthEnemyFoldout);

        // EnemyInfo _firstEnemyInfo = new();
        // EnemyInfo _secondEnemyInfo = new();
        // EnemyInfo _firstEnemyInfo = new();
        // EnemyInfo _firstEnemyInfo = new();
        // EnemyInfo _firstEnemyInfo = new();
        // EnemyInfo _firstEnemyInfo = new();

        _firstEnemyInfo.Add(_firstEnemyDropdown, _firstEnemyStatsField);
        _secondEnemyInfo.Add(_secondEnemyDropdown, _secondEnemyStatsField);
        _thirdEnemyInfo.Add(_thirdEnemyDropdown, _thirdEnemyStatsField);
        _fourthEnemyInfo.Add(_fourthEnemyDropdown, _fourthEnemyStatsField);
        _fifthEnemyInfo.Add(_fifthEnemyDropdown, _fifthEnemyStatsField);
        _sixthEnemyInfo.Add(_sixthEnemyDropdown, _sixthEnemyStatsField);

        // _enemiesInfo.Add(_firstEnemyInfo);
        // _enemiesInfo.Add(_secondEnemyInfo);
        // _enemiesInfo.Add(_thirdEnemyInfo);
        // _enemiesInfo.Add(_fourthEnemyInfo);
        // _enemiesInfo.Add(_fifthEnemyInfo);
        // _enemiesInfo.Add(_sixthEnemyInfo);

        // Set Base value for the foldouts
        foreach (Foldout foldout in _enemiesFoldout)
        {
            foldout.text = "Enemy Stats";
        }
    }

    public void OnGUI()
    {

        EnemyLoader enemyLoader = new();
        _enemies = enemyLoader.LoadEnemies("Assets/Resources/CSV/Enemies.csv");

        foreach (DropdownField field in _enemiesDropdown)
        {
            foreach (Enemy enemy in _enemies)
            {
                field.choices.Add("No Enemy");
                field.choices.Add(enemy.Name);
            }
        }

        _supports = new List<SupportCharacterSO>(Resources.LoadAll<SupportCharacterSO>("SO/SupportsCharacter"));
        // TODO: Refactore it
        foreach (SupportCharacterSO support in _supports)
        {
            _firstSupportDropdown.choices.Add("No Support");
            _firstSupportDropdown.choices.Add(support.Name);
            _secondSupportDropdown.choices.Add("No Support");
            _secondSupportDropdown.choices.Add(support.Name);
        }

        _weapons = new List<GearSO>(Resources.LoadAll<GearSO>("SO/Weapons"));
        foreach (GearSO weapon in _weapons)
        {
            _weaponDropdown.choices.Add("No Weapon");
            _weaponDropdown.choices.Add(weapon.Name);
        }

        _gears = new List<GearSO>(Resources.LoadAll<GearSO>("SO/GearsCreator"));
        // loop trough all the gears and filter them by type
        foreach (GearSO gear in _gears)
        {
            switch (gear.Type)
            {
                case Gear.GearType.Helmet:
                    _helmetDropdown.choices.Add("No Helmet");
                    _helmetDropdown.choices.Add(gear.Name);
                    break;
                case Gear.GearType.Chest:
                    _chestDropdown.choices.Add("No Chest");
                    _chestDropdown.choices.Add(gear.Name);
                    break;
                case Gear.GearType.Boots:
                    _bootsDropdown.choices.Add("No Boots");
                    _bootsDropdown.choices.Add(gear.Name);
                    break;
                case Gear.GearType.Ring:
                    _ringDropdown.choices.Add("No Ring");
                    _ringDropdown.choices.Add(gear.Name);
                    break;
                case Gear.GearType.Necklace:
                    _necklaceDropdown.choices.Add("No Necklace");
                    _necklaceDropdown.choices.Add(gear.Name);
                    break;
                case Gear.GearType.Earrings:
                    _earringsDropdown.choices.Add("No Earrings");
                    _earringsDropdown.choices.Add(gear.Name);
                    break;
            }
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

                GearSO weapon = _weapons.Find(x => x.Name == evt.newValue);

                ChangeFieldsOfWeapon(evt.newValue, _weaponSkillField, _weaponStatField);
            }
            else
            {
                _weaponFoldout.visible = false;
            }
        });

        // GEARS
        _helmetDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Helmet")
            {
                _helmetFoldout.visible = true;
                _helmetFoldout.text = evt.newValue;

                ChangeFieldsOfGear(evt.newValue, _helmetStatField);
            }
            else
            {
                _helmetFoldout.visible = false;
            }
        });

        _chestDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Chest")
            {
                _chestFoldout.visible = true;
                _chestFoldout.text = evt.newValue;

                ChangeFieldsOfGear(evt.newValue, _chestStatField);
            }
            else
            {
                _chestFoldout.visible = false;
            }
        });

        _bootsDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Boots")
            {
                _bootsFoldout.visible = true;
                _bootsFoldout.text = evt.newValue;

                ChangeFieldsOfGear(evt.newValue, _bootsStatField);
            }
            else
            {
                _bootsFoldout.visible = false;
            }
        });

        _ringDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Ring")
            {
                _ringFoldout.visible = true;
                _ringFoldout.text = evt.newValue;

                ChangeFieldsOfGear(evt.newValue, _ringStatField);
            }
            else
            {
                _ringFoldout.visible = false;
            }
        });

        _necklaceDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Necklace")
            {
                _necklaceFoldout.visible = true;
                _necklaceFoldout.text = evt.newValue;

                ChangeFieldsOfGear(evt.newValue, _necklaceStatField);
            }
            else
            {
                _necklaceFoldout.visible = false;
            }
        });

        _earringsDropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != "No Earrings")
            {
                _earringsFoldout.visible = true;
                _earringsFoldout.text = evt.newValue;

                ChangeFieldsOfGear(evt.newValue, _earringsStatField);
            }
            else
            {
                _earringsFoldout.visible = false;
            }
        });

        // SUPPORTS
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

        int i = 0;
        foreach (var enemyInfo in _enemiesInfo)
        {
            foreach (var item in enemyInfo)
            {
                foreach (Foldout foldout in _enemiesFoldout)
                {
                    item.Key.RegisterValueChangedCallback(evt =>
                    {
                        if (evt.newValue != "No Enemy")
                        {
                            foldout.visible = true;
                            foldout.text = evt.newValue;

                            ChangeFieldsOfEnemy(evt.newValue, item.Value, i);
                        }
                        else
                        {
                            foldout.visible = false;
                        }

                    });
                }
            }
            i++;
        }


    }

    private void ChangeFieldsOfEnemy(string enemyName, List<IntegerField> enemyStatsField, int index)
    {
        // TODO: To Enemy 
        Entity enemy = _enemies.Find(x => x.Name == enemyName);


        _selectedEnemies.Add(enemy);
        


        foreach (var stat in enemy.Stats)
        {
            foreach (var field in enemyStatsField)
            {
                if (field.name == stat.Key.ToString())
                {
                    field.SetValueWithoutNotify((int)stat.Value.Value);
                }
            }
        }
    }

    private void ChangeFieldsOfSupport(string supportName, List<TextField> supportFields)
    {
        SupportCharacterSO support = _supports.Find(x => x.Name == supportName);
        supportFields[0].label = support.PrimarySkillData.IsPassive ? "Passive" : "Active";
        supportFields[1].label = support.SecondarySkillData.IsPassive ? "Passive" : "Active";

        // set the value of the skill to the name of the skill
        supportFields[0].SetValueWithoutNotify(support.PrimarySkillData.Name);
        supportFields[1].SetValueWithoutNotify(support.SecondarySkillData.Name);
    }
    private void ChangeFieldsOfGear(string gearName, List<IntegerField> gearStatFields)
    {
        GearSO gear = _gears.Find(x => x.Name == gearName);

        // Change the name of all the stat field to the name of the stat
        gearStatFields[0].label = gear.Attribute.ToString();
        gearStatFields[0].SetValueWithoutNotify((int)gear.StatValue);

        foreach (var substat in gear.Substats)
        {
            foreach (var field in gearStatFields)
            {
                field.label = substat.Key.ToString();
                field.SetValueWithoutNotify((int)substat.Value);
            }
        }
    }

    private void ChangeFieldsOfWeapon(string weaponName, List<TextField> weaponSkillFields, List<IntegerField> weaponStatFields)
    {
        GearSO weapon = _weapons.Find(x => x.Name == weaponName);

        weaponStatFields[0].label = weapon.Attribute.ToString();
        weaponStatFields[0].SetValueWithoutNotify((int)weapon.StatValue);

        weaponSkillFields[0].label = weapon.FirstSkillData.IsPassive ? "Passive" : "Active";
        weaponSkillFields[1].label = weapon.SecondSkillData.IsPassive ? "Passive" : "Active";

        weaponSkillFields[0].SetValueWithoutNotify(weapon.FirstSkillData.Name);
        weaponSkillFields[1].SetValueWithoutNotify(weapon.SecondSkillData.Name);
    }

    private void OnInspectorUpdate()
    {
        // Repaint();
        _simulateButton.clicked += () =>
        {
            Instance.SimulateBattle();
            Debug.Log("Simulate");
        };
    }
}

public class EnemyInfo
{
    public DropdownField dropdowns;
    public List<IntegerField> stats;
    public Foldout foldouts;
}