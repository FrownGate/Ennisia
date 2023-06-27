using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Linq;
public class BattleSimulator : EditorWindow
{
    public class EnemyInfo
    {
        public DropdownField Dropdown;
        public Dictionary<Item.AttributeStat, IntegerField> Stats;
        public Foldout Foldout;
    }
    public class GearInfo
    {
        public DropdownField Dropdown;
        public Dictionary<Item.AttributeStat?, IntegerField> Stats;
        public Foldout Foldout;
        public List<IntegerField> IntegerFields;
    }
    List<GroupBox> _gearsGroupbox;
    List<GroupBox> _enemiesGroupbox;
    //TODO: -> Optimize stored datas with enum, arrays, loops, etc.

    public static BattleSystem Instance;
    private Player _player;
    private List<SupportCharacterSO> _supports = new();
    private List<Gear> _weapons = new();
    // TODO: To Enemy 
    private List<Entity> _enemies = new();
    private List<Gear> _gears = new();
    // TODO: To Enemy 
    private List<Entity> _selectedEnemies = new();
    private List<SupportCharacterSO> _selectedSupports = new();
    private Dictionary<Item.GearType, Gear> _selectedGears = new();
    private Gear _selectedWeapon;

    private readonly List<EnemyInfo> _enemiesInfos = new();
    private readonly List<GearInfo> _gearsInfos = new();



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

        _weaponGroupBox = root.Q<GroupBox>("Weapon");

        _weaponStatField.Add(_weaponGroupBox.Q<IntegerField>("MainStat"));
        _weaponSkillField.Add(_weaponGroupBox.Q<TextField>("Skill1"));
        _weaponSkillField.Add(_weaponGroupBox.Q<TextField>("Skill2"));

        _weaponDropdown = root.Q<DropdownField>("weapon-dropdown");

        _weaponFoldout = root.Q<Foldout>("weapon-foldout");
        _weaponFoldout.text = "Weapon Skills";

        _enemiesGroupbox = root.Query<GroupBox>("EnemyGroupbox").ToList();

        foreach (var item in _enemiesGroupbox)
        {
            EnemyInfo enemyInfo = new()
            {
                Dropdown = item.Q<DropdownField>("EnemyDropdown"),
                Foldout = item.Q<Foldout>("EnemyFoldout"),
                Stats = new()
            };

            List<IntegerField> statList = item.Query<IntegerField>().ToList();

            foreach (IntegerField stat in statList)
            {
                enemyInfo.Stats.Add((Item.AttributeStat)Enum.Parse(typeof(Item.AttributeStat), stat.label), stat);
            }
            _enemiesInfos.Add(enemyInfo);
        }

        foreach (EnemyInfo enemyInfo in _enemiesInfos)
        {
            enemyInfo.Foldout.text = "Enemy Stats";
        }

        List<GearSO> gearsSO = new(Resources.LoadAll<GearSO>("SO/GearsCreator"));

        foreach (var gear in gearsSO)
        {
            Gear NewGear = new(gear);
            _gears.Add(NewGear);
            foreach (var info in _gearsInfos)
            {
                if (gear.Type.ToString() == info.Dropdown.label)
                {
                    info.Dropdown.choices.Add("No Gear");
                    info.Dropdown.choices.Add(NewGear.Name);
                }
            }
        }

        _gearsGroupbox = root.Query<GroupBox>("GearGroupbox").ToList();
        foreach (var item in _gearsGroupbox)
        {
            GearInfo gearInfo = new()
            {
                Dropdown = item.Q<DropdownField>("GearDropdown"),
                Foldout = item.Q<Foldout>("GearFoldout"),
                Stats = new(),
                IntegerFields = item.Query<IntegerField>().ToList()
            };

            // loop through all the IntegerFields and add them to the dictionnary with the name of the stat as a key from _gears
            foreach (var gear in _gears)
            {
                gearInfo.Stats[(Item.AttributeStat)gear.Attribute] = gearInfo.IntegerFields[0];

                for (int i = 1; i < gearInfo.IntegerFields.Count; i++)
                {
                    if (gear.Substats.Count <= i - 1)
                    {
                        break;
                    }
                    gearInfo.Stats[(Item.AttributeStat)gear.Substats.ToArray()[i - 1].Key] = gearInfo.IntegerFields[i];
                }
            }
            _gearsInfos.Add(gearInfo);
        }

    }

    public void OnGUI()
    {
        _player = new();
        EnemyLoader enemyLoader = new();
        _enemies = enemyLoader.LoadEnemies("Assets/Resources/CSV/Enemies.csv");

        foreach (EnemyInfo field in _enemiesInfos)
        {
            foreach (Entity enemy in _enemies)
            {
                field.Dropdown.choices.Add("No Enemy");
                field.Dropdown.choices.Add(enemy.Name);
            }
        }

        _supports = new List<SupportCharacterSO>(Resources.LoadAll<SupportCharacterSO>("SO/SupportsCharacter"));
        foreach (SupportCharacterSO support in _supports)
        {
            _firstSupportDropdown.choices.Add("No Support");
            _firstSupportDropdown.choices.Add(support.Name);
            _secondSupportDropdown.choices.Add("No Support");
            _secondSupportDropdown.choices.Add(support.Name);
        }

        List<GearSO> weaponsSO = new(Resources.LoadAll<GearSO>("SO/Weapons"));
        foreach (GearSO weapon in weaponsSO)
        {
            Gear NewWeapon = new(weapon);
            _weapons.Add(NewWeapon);
            _weaponDropdown.choices.Add("No Weapon");
            _weaponDropdown.choices.Add(NewWeapon.Name);
        }

        List<GearSO> gearsSO = new(Resources.LoadAll<GearSO>("SO/GearsCreator"));

        foreach (var gear in gearsSO)
        {
            Gear NewGear = new(gear);
            _gears.Add(NewGear);
            foreach (var info in _gearsInfos)
            {
                if (gear.Type.ToString() == info.Dropdown.label)
                {
                    info.Dropdown.choices.Add("No Gear");
                    info.Dropdown.choices.Add(NewGear.Name);
                }
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


                ChangeFieldsOfWeapon(evt.newValue, _weaponSkillField, _weaponStatField);
            }
            else
            {
                _weaponFoldout.visible = false;
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

        foreach (var info in _gearsInfos)
        {
            List<IntegerField> statFields = new();
            foreach (var stats in info.Stats)
            {
                Debug.LogWarning("Key : " + stats.Key.ToString());
                Debug.LogWarning("Value : " + stats.Value.ToString());
                statFields.Add(stats.Value);

                info.Dropdown.RegisterValueChangedCallback(evt =>
                {
                    if (evt.newValue != "No Gear")
                    {
                        info.Foldout.visible = true;
                        info.Foldout.text = evt.newValue;
                        ChangeFieldsOfGear(evt.newValue, statFields);
                    }
                    else
                    {
                        info.Foldout.visible = false;
                    }
                });
            }
        }

        int i = 0;
        foreach (var info in _enemiesInfos)
        {
            foreach (var stats in info.Stats)
            {
                List<IntegerField> statFields = info.Foldout.Query<IntegerField>().ToList();
                info.Dropdown.RegisterValueChangedCallback(evt =>
                {
                    if (evt.newValue != "No Enemy")
                    {
                        info.Foldout.visible = true;
                        info.Foldout.text = evt.newValue;
                        ChangeFieldsOfEnemy(evt.newValue, statFields, i);
                    }
                    else
                    {
                        info.Foldout.visible = false;
                    }

                });
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

        supportFields[0].SetValueWithoutNotify(support.PrimarySkillData.Name);
        supportFields[1].SetValueWithoutNotify(support.SecondarySkillData.Name);
        Debug.Log(support);
        _selectedSupports.Add(support);
    }
    private void ChangeFieldsOfGear(string gearName, List<IntegerField> gearStatFields)
    {
        Gear gear = _gears.Find(x => x.Name == gearName);

        // Change the name of all the stat field to the name of the stat
        gearStatFields[0].label = gear.Attribute.ToString();
        gearStatFields[0].SetValueWithoutNotify((int)gear.Value);



        foreach (var info in _gearsInfos)
        {
            foreach (var stat in info.Stats)
            {
                if (stat.Key != null)
                {
                    gearStatFields[1].label = stat.Key.ToString();
                    gearStatFields[1].SetValueWithoutNotify((int)stat.Value.value);
                }
                else
                {
                    gearStatFields[1].visible = false;
                }
            }
        }


        _selectedGears[(Item.GearType)gear.Type] = gear;

    }

    private void ChangeFieldsOfWeapon(string weaponName, List<TextField> weaponSkillFields, List<IntegerField> weaponStatFields)
    {
        Gear weapon = _weapons.Find(x => x.Name == weaponName);


        weaponStatFields[0].label = weapon.Attribute.ToString();
        weaponStatFields[0].SetValueWithoutNotify((int)weapon.Value);

        weaponSkillFields[0].label = weapon.WeaponSO.FirstSkillData.IsPassive ? "Passive" : "Active";
        weaponSkillFields[1].label = weapon.WeaponSO.SecondSkillData.IsPassive ? "Passive" : "Active";

        weaponSkillFields[0].SetValueWithoutNotify(weapon.WeaponSO.FirstSkillData.Name);
        weaponSkillFields[1].SetValueWithoutNotify(weapon.WeaponSO.SecondSkillData.Name);



    }

    private void OnInspectorUpdate()
    {
        // Repaint();
        _simulateButton.clicked += () =>
        {
            Debug.Log(_player.Name);
            Debug.Log(_selectedWeapon.Name);
            foreach (var support in _selectedSupports)
            {
                Debug.LogError(support.Name);
            }
            // make a list of the _selectedGears
            List<Gear> tempGears = new();
            foreach (var gear in _selectedGears)
            {
                tempGears.Add(gear.Value);
            }



            Instance.SimulateBattle(_player, _selectedEnemies, _selectedSupports, _selectedWeapon, tempGears);
        };
    }
}