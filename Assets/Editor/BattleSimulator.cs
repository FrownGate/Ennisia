using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;

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

    private readonly List<Data> _gearsData = new();
    private Data _weaponData;
    private readonly List<Data> _supportsData = new();
    private readonly List<Data> _enemiesData = new();

    private Player _player;
    private readonly List<Gear> _gears = new();
    private readonly List<Gear> _weapons = new();
    private List<SupportCharacterSO> _supports;
    private List<Entity> _enemies;

    private readonly Dictionary<GearType, Gear> _selectedGears = new();
    private Gear _selectedWeapon;
    private readonly List<SupportCharacterSO> _selectedSupports = new();
    private readonly List<Entity> _selectedEnemies = new();

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

        BattleSystem.OnBattleLoaded += StartBattle;
        _simulateButton = root.Q<Button>("simulate-button");
        _simulateButton.clicked += CreateBattle;

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
        foreach (var weapon in weaponsSO) _weapons.Add(new(weapon));
        GroupBox weaponGroupbox = root.Q<GroupBox>("Weapon");

        _weaponData = new()
        {
            Dropdown = root.Q<DropdownField>("weapon-dropdown"),
            Foldout = root.Q<Foldout>("weapon-foldout"),
            IntegerFields = weaponGroupbox.Query<IntegerField>().ToList(),
            TextFields = weaponGroupbox.Query<TextField>().ToList()
        };

        _weaponData.Dropdown.choices.Add(_empty);
        _weaponData.Dropdown.value = _empty;
        _weaponData.Foldout.text = "Weapon Skills";

        foreach (Gear weapon in _weapons) _weaponData.Dropdown.choices.Add(weapon.Name);

        HideData(_weaponData);
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

    private void OnDestroy()
    {
        BattleSystem.OnBattleLoaded -= StartBattle;
        _simulateButton.clicked -= CreateBattle;
    }

    private void ChangeFoldoutOnDropdown()
    {
        //Gears
        foreach (var data in _gearsData)
        {
            data.Dropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != _empty)
                {
                    data.Foldout.visible = true;
                    data.Foldout.text = evt.newValue;
                    ChangeGearFields(data);
                    return;
                }

                HideData(data);
            });
        }

        //Weapon
        _weaponData.Dropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue != _empty)
            {
                _weaponData.Foldout.visible = true;
                _weaponData.Foldout.text = evt.newValue;
                ChangeWeaponFields();
                return;
            }

            HideData(_weaponData);
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
                if (stat.Key == Attribute.DefIgnored) continue;
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
            int textFieldIndex = 0;

            foreach (var skillData in supportSO.SkillsData)
            {
                var skill = supportSO.Skills.FirstOrDefault(s => s.GetType().Name == skillData.Name);
                if (skill == null) continue;

                supportData.TextFields[textFieldIndex].visible = true;
                supportData.TextFields[textFieldIndex].label = skillData.IsPassive ? "Passive" : "Active";
                supportData.TextFields[textFieldIndex].SetValueWithoutNotify(skillData.Name);

                textFieldIndex++;

                if (textFieldIndex >= supportData.TextFields.Count)
                    break;
            }

            for (int i = textFieldIndex; i < supportData.TextFields.Count; i++)
            {
                supportData.TextFields[i].visible = false;
            }
        }
    }

    private void ChangeGearFields(Data gearData)
    {
        Gear gear = _gears.Find(x => x.Name == gearData.Dropdown.value);
        _selectedGears[(GearType)gear.Type] = gear;

        ChangeStatField(gearData.IntegerFields[0], gear.Attribute.ToString(), (int)gear.Value);
        int index = 1;

        foreach (var substat in gear.SubStats)
        {
            ChangeStatField(gearData.IntegerFields[index], substat.Key.ToString(), (int)substat.Value);
            index++;
        }

        if (gear.SubStats.Count == gearData.IntegerFields.Count) return;

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

    private void ChangeWeaponFields()
    {
        Gear weapon = _weapons.Find(x => x.Name == _weaponData.Dropdown.value);
        _selectedWeapon = weapon;

        _weaponData.IntegerFields[0].visible = true;
        _weaponData.IntegerFields[0].label = weapon.Attribute.ToString();
        _weaponData.IntegerFields[0].SetValueWithoutNotify((int)weapon.Value);

        int textFieldIndex = 0;
        foreach (var field in _weaponData.TextFields)
        {
            field.visible = true;

            if (textFieldIndex >= weapon.WeaponSO.SkillsData.Count)
                break;

            var skillData = weapon.WeaponSO.SkillsData[textFieldIndex];
            var skill = weapon.WeaponSO.Skills.FirstOrDefault(s => s.GetType().Name == skillData.Name);
            if (skill == null) continue;

            field.label = skillData.IsPassive ? "Passive" : "Active";
            field.SetValueWithoutNotify(skillData.Name);

            textFieldIndex++;
        }

        for (int i = textFieldIndex; i < _weaponData.TextFields.Count; i++)
        {
            _weaponData.TextFields[i].visible = false;
        }
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

    private void CreateBattle()
    {
        if (_selectedWeapon == null)
        {
            Debug.LogError("You need to choose a weapon.");
            return;
        }

        bool hasEnemy = false;

        foreach (var enemy in _selectedEnemies)
        {
            if (enemy != null)
            {
                hasEnemy = true;
                break;
            }
        }

        if (!hasEnemy)
        {
            Debug.LogError("You need to choose at least 1 enemy.");
            return;
        }

        _player = new()
        {
            Weapon = _selectedWeapon.WeaponSO
        };

        ScenesManager.Instance.SetScene("Battle");
    }

    private void StartBattle(BattleSystem battleSystem)
    {
        battleSystem.SimulateBattle(_player, _selectedEnemies);
    }
}