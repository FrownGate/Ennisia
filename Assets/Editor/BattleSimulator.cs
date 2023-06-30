using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class BattleSimulator : EditorWindow
{
    public class Data
    {
        public int Id;
        public DropdownField Dropdown;
        public Foldout Foldout;
        public List<IntegerField> IntegerFields;
        public List<TextField> TextFields;
    }

    private readonly List<Data> _enemiesData = new();
    private readonly List<Data> _gearsData = new();
    private readonly List<Data> _supportsData = new();

    private readonly List<Gear> _gears = new();
    private List<SupportCharacterSO> _supports = new();
    private List<Entity> _enemies; // TODO: To Enemy

    private readonly Dictionary<Item.GearType, Gear> _selectedGears = new();
    private readonly List<SupportCharacterSO> _selectedSupports = new();
    private readonly List<Entity> _selectedEnemies = new(); // TODO: To Enemy 

    private Player _player;

    public static BattleSystem Instance;
    private List<Gear> _weapons = new();
    private Gear _selectedWeapon;

    // WEAPON
    private GroupBox _weaponGroupBox;
    private DropdownField _weaponDropdown;
    private Foldout _weaponFoldout;
    private readonly List<IntegerField> _weaponStatField = new();
    private readonly List<TextField> _weaponSkillField = new();

    private Button _simulateButton;
    private static readonly string _toolName = "Battle Simulator";
    private readonly string _empty = "None";

    [MenuItem("Tools/Battle Simulator")]
    public static void ShowWindow()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogError($"You must be in play mode to use the {_toolName}");
            return;
        }

        BattleSimulator window = GetWindow<BattleSimulator>(_toolName);
        GUIContent icon = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow");
        window.titleContent = new GUIContent(_toolName, icon.image, _toolName);
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 1000, 1000);
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BattleSimulator.uxml");
        root.Add(visualTree.CloneTree());

        _simulateButton = root.Q<Button>("simulate-button");

        _weaponGroupBox = root.Q<GroupBox>("Weapon");

        _weaponStatField.Add(_weaponGroupBox.Q<IntegerField>("MainStat"));
        _weaponSkillField.Add(_weaponGroupBox.Q<TextField>("Skill1"));
        _weaponSkillField.Add(_weaponGroupBox.Q<TextField>("Skill2"));

        _weaponDropdown = root.Q<DropdownField>("weapon-dropdown");

        _weaponFoldout = root.Q<Foldout>("weapon-foldout");
        _weaponFoldout.text = "Weapon Skills";

        #region Gears
        List<GearSO> gearsSO = new(Resources.LoadAll<GearSO>("SO/GearsCreator"));
        foreach (var gear in gearsSO) _gears.Add(new(gear));

        foreach (var item in root.Query<GroupBox>("GearGroupbox").ToList())
        {
            Data gearData = new()
            {
                Dropdown = item.Q<DropdownField>("GearDropdown"),
                Foldout = item.Q<Foldout>("GearFoldout"),
                IntegerFields = item.Query<IntegerField>().ToList()
            };

            gearData.Dropdown.choices.Add(_empty);
            gearData.Dropdown.value = _empty;

            // loop through all the IntegerFields and add them to the dictionnary with the name of the stat as a key from _gears
            foreach (var gear in _gears)
            {
                if (gear.Type.ToString() == gearData.Dropdown.label) gearData.Dropdown.choices.Add(gear.Name);
            }

            HideData(gearData);
            _gearsData.Add(gearData);
        }
        #endregion

        #region Weapon
        List<GearSO> weaponsSO = new(Resources.LoadAll<GearSO>("SO/Weapons"));
        foreach (GearSO weapon in weaponsSO)
        {
            Gear NewWeapon = new(weapon);
            _weapons.Add(NewWeapon);
            _weaponDropdown.choices.Add("No Weapon");
            _weaponDropdown.choices.Add(NewWeapon.Name);
        }
        #endregion

        #region Supports
        _supports = new(Resources.LoadAll<SupportCharacterSO>("SO/SupportsCharacter"));

        int id = 0;
        foreach (var item in root.Query<GroupBox>("SupportGroupbox").ToList())
        {
            Data supportData = new()
            {
                Id = id,
                Dropdown = item.Q<DropdownField>("SupportDropdown"),
                Foldout = item.Q<Foldout>("SupportFoldout"),
                TextFields = item.Query<TextField>().ToList()
            };

            supportData.Dropdown.choices.Add(_empty);
            supportData.Dropdown.value = _empty;
            supportData.Foldout.text = "Support Skills";

            foreach (SupportCharacterSO support in _supports) supportData.Dropdown.choices.Add(support.Name);

            HideData(supportData);
            _supportsData.Add(supportData);
            id++;
        }
        #endregion

        #region Enemies
        _enemies = new EnemyLoader().LoadEnemies("Assets/Resources/CSV/Enemies.csv");

        foreach (var item in root.Query<GroupBox>("EnemyGroupbox").ToList())
        {
            Data enemyData = new()
            {
                Dropdown = item.Q<DropdownField>("EnemyDropdown"),
                Foldout = item.Q<Foldout>("EnemyFoldout"),
                IntegerFields = item.Query<IntegerField>().ToList()
            };

            enemyData.Dropdown.choices.Add(_empty);
            enemyData.Dropdown.value = _empty;
            enemyData.Foldout.text = "Enemy Stats";

            foreach (Entity enemy in _enemies) enemyData.Dropdown.choices.Add(enemy.Name);

            HideData(enemyData);
            _enemiesData.Add(enemyData);
        }
        #endregion

        ChangeFoldoutOnDropdown();
    }

    private void OnGUI()
    {
        if (EditorApplication.isPlaying) return;
        Close();
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

        //Supports
        foreach (var data in _supportsData)
        {
            data.Dropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != _empty)
                {
                    data.Foldout.visible = true;
                    data.Foldout.text = evt.newValue;
                    ChangeSupportFields(data);
                    return;
                }

                HideData(data);
            });
        }

        //Gears
        foreach (var data in _gearsData)
        {
            data.Dropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != _empty)
                {
                    data.Foldout.visible = true;
                    data.Foldout.text = evt.newValue;
                    ChangeGearFields(evt.newValue, data);
                    return;
                }

                HideData(data);
            });
        }

        //Enemies
        foreach (var data in _enemiesData)
        {
            data.Dropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != _empty)
                {
                    data.Foldout.visible = true;
                    data.Foldout.text = evt.newValue;
                    ChangeEnemyFields(data);
                    return;
                }
                
                HideData(data);
            });
        }
    }

    private void ChangeEnemyFields(Data enemyData)
    {
        _selectedEnemies.Clear();

        foreach (var enemy in _enemiesData)
        {
            Entity enemySO = _enemies.Find(x => x.Name == enemy.Dropdown.value);
            _selectedEnemies.Add(enemySO ?? null);

            if (enemy != enemyData) continue;

            int index = 0;

            foreach (var stat in enemySO.Stats)
            {
                if (stat.Key == Item.AttributeStat.DefIgnoref) continue;
                ChangeStatField(enemyData.IntegerFields[index], stat.Key.ToString(), (int)stat.Value.Value);
                index++;
            }
        }

        //foreach (var enemy in _selectedEnemies) Debug.Log(enemy != null ? enemy.Name : _empty);
    }

    private void ChangeSupportFields(Data supportData)
    {
        _selectedSupports.Clear();

        foreach (var support in _supportsData)
        {
            SupportCharacterSO supportSO = _supports.Find(x => x.Name == support.Dropdown.value);
            _selectedSupports.Add(supportSO ?? null);

            if (support != supportData) continue;

            supportData.TextFields[0].visible = true;
            supportData.TextFields[0].label = supportSO.PrimarySkillData.IsPassive ? "Passive" : "Active";
            supportData.TextFields[0].SetValueWithoutNotify(supportSO.PrimarySkillData.Name);

            supportData.TextFields[1].visible = supportSO.SecondarySkillData != null;
            if (supportSO.SecondarySkillData == null) continue;

            supportData.TextFields[1].label = supportSO.SecondarySkillData.IsPassive ? "Passive" : "Active";
            supportData.TextFields[1].SetValueWithoutNotify(supportSO.SecondarySkillData.Name);
        }
    }
    private void ChangeGearFields(string gearName, Data gearData)
    {
        Gear gear = _gears.Find(x => x.Name == gearName);
        _selectedGears[(Item.GearType)gear.Type] = gear;

        ChangeStatField(gearData.IntegerFields[0], gear.Attribute.ToString(), (int)gear.Value);
        int index = 1;

        foreach (var substat in gear.Substats)
        {
            ChangeStatField(gearData.IntegerFields[index], substat.Key.ToString(), (int)substat.Value);
            index++;
        }

        if (gear.Substats.Count == gearData.IntegerFields.Count) return;

        for (int i = index; i < gearData.IntegerFields.Count; i++)
        {
            gearData.IntegerFields[i].visible = false;
        }
    }

    private void ChangeStatField(IntegerField field, string label, int value)
    {
        field.visible = true;
        field.label = label;
        field.SetValueWithoutNotify(value);
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

    private void HideData(Data data)
    {
        if (data.TextFields != null)
        {
            foreach (var field in data.TextFields) field.visible = false;
        }

        if (data.IntegerFields != null)
        {
            foreach (var field in data.IntegerFields) field.visible = false;
        }

        data.Foldout.visible = false;
    }

    private void OnInspectorUpdate()
    {
        // Repaint();
        _simulateButton.clicked += () =>
        {
            _player = new();
            Debug.LogWarning(_player.Name);
            Debug.LogWarning(_selectedWeapon.Name);
            Debug.LogWarning(_selectedSupports.Count);
            // foreach (var support in _selectedSupports)
            // {
            //     Debug.LogWarning(support.Name);
            // }
            // make a list of the _selectedGears
            List<Gear> tempGears = new();
            foreach (var gear in _selectedGears)
            {
                tempGears.Add(gear.Value);
            }

            //Instance.SimulateBattle(_player, _selectedEnemies, _selectedSupports, _selectedWeapon, tempGears);
        };
    }
}