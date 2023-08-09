using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class PlayerData
{
    public string Name;
    public int Level;
    public int Exp;
    public int[] EquippedGearsId;
    public string[] EquippedSupportsPath;
    [NonSerialized] public SupportCharacterSO[] EquippedSupports;
    [NonSerialized] public readonly Dictionary<GearType, Gear> EquippedGears = new();
    [NonSerialized] public readonly Dictionary<Attribute, Stat<float>> Stats = new();
    [NonSerialized] private readonly Dictionary<GearType, Dictionary<Attribute, List<ModifierID>>> _modifiers = new();
    public Dictionary<int, int> PlayerlevelExperienceMap;
    public static event Action<int, LevelUpQuestEvent.LvlType> OnPlayerLevelUp;

    public PlayerData()
    {
        Level = 1;
        Exp = 0;
        EquippedGearsId = new int[7];
        EquippedSupportsPath = new string[2] { null, null };
        EquippedSupports = new SupportCharacterSO[2] { null, null };

        foreach (var item in Enum.GetNames(typeof(GearType)))
        {
            EquippedGears[Enum.Parse<GearType>(item)] = null;
            _modifiers[Enum.Parse<GearType>(item)] = new();
        }

        foreach (string stat in Enum.GetNames(typeof(Attribute)))
        {
            Stats[Enum.Parse<Attribute>(stat)] = new(1);
        }
        LoadLevelExperienceMap();
        InitModifiers();
    }

    private void InitModifiers()
    {
        foreach (var item in Enum.GetNames(typeof(GearType)))
        {
            _modifiers[Enum.Parse<GearType>(item)] = new();

            foreach (string stat in Enum.GetNames(typeof(Attribute)))
            {
                _modifiers[Enum.Parse<GearType>(item)][Enum.Parse<Attribute>(stat)] = new();
            }
        }
    }

    public void UpdateEquippedSupports()
    {
        for (int i = 0; i < EquippedSupportsPath.Length; i++)
        {
            EquippedSupports[i] = EquippedSupportsPath[i] != null ? Resources.Load<SupportCharacterSO>(EquippedSupportsPath[i]) : null;
            Debug.Log($"Equipped Support #{i + 1} = {(EquippedSupports[i] != null ? EquippedSupports[i].Name : "None")}");
        }
    }

    private void EquipGear(Gear gear)
    {
        RemoveModifiers((GearType)gear.Type);
        EquippedGearsId[(int)gear.Type] = gear.Id;
        EquippedGears[(GearType)gear.Type] = gear;

        ModifierID id = Stats[(Attribute)gear.Attribute].AddModifier((float value) => value + gear.Value, 0);
        _modifiers[(GearType)gear.Type][(Attribute)gear.Attribute].Add(id);

        Debug.Log(gear.Attribute);
        Debug.Log(gear.Value);

        if (gear.Type == GearType.Weapon) gear.WeaponSO.Init();

        if (gear.SubStats == null) return;

        foreach (var substat in gear.SubStats)
        {
            id = Stats[substat.Key].AddModifier((float value) => value + substat.Value, 0);
            _modifiers[(GearType)gear.Type][substat.Key].Add(id);
        }
    }

    public void Equip(Gear gear, bool update = true)
    {
        EquipGear(gear);
        if (update) PlayFabManager.Instance.UpdateData();
    }

    public void Equip(List<Gear> gears, bool update = true)
    {
        foreach (Gear gear in gears)
        {
            EquipGear(gear);
        }

        if (update) PlayFabManager.Instance.UpdateData();
    }

    public void Equip(SupportCharacterSO support, int slot = 0, bool update = true)
    {
        if (slot < 0 || slot > 1)
        {
            Debug.LogError("Support slot is invalid (must be 0 or 1).");
            return;
        }

        EquippedSupportsPath[slot] = $"SO/SupportsCharacter/{support.Rarity}/{support.name}";
        EquippedSupports[slot] = support;
        EquippedSupports[slot].Init();
        if (update) PlayFabManager.Instance.UpdateData();
    }

    public void Unequip(GearType type, bool update = true)
    {
        Debug.Log("unequipped gear :" + type + "|id : " + (int)type);
        RemoveModifiers(type);
        EquippedGearsId[(int)type] = 0;
        EquippedGears[type] = null;
        if (update) PlayFabManager.Instance.UpdateData();
    }

    public void Unequip(int slot, bool update = true)
    {
        if (slot < 0 || slot > 1)
        {
            Debug.LogError("Support slot is invalid (must be 0 or 1).");
            return;
        }

        EquippedSupportsPath[slot] = null;
        EquippedSupports[slot] = null;
        if (update) PlayFabManager.Instance.UpdateData();
    }

    private void RemoveModifiers(GearType type)
    {
        Debug.Log(type + "type");

        if (EquippedGears[type] == null) return;
        foreach (var attribute in _modifiers[type])
        {
            foreach (var modifier in attribute.Value)
            {
                Stats[attribute.Key].RemoveModifier(modifier);
            }
        }
    }

    public void UpdatePlayerStats()
    {
        Dictionary<Attribute, float> basStats = PlayFabManager.Instance.PlayerBaseStats;

        foreach (var stat in basStats)
        {
            //Debug.Log("Base stats : " + PlayFabManager.Instance.PlayerBaseStats[stat.Key] + stat.Key);
            Stats[stat.Key] = new(PlayFabManager.Instance.PlayerBaseStats[stat.Key] + PlayFabManager.Instance.PlayerBaseStats[stat.Key] * CalculationChoice(stat.Key));
        }
    }

    private float CalculationChoice(Attribute stat)
    {
        switch (stat)
        {
            case Attribute.HP: return Level / 2.5f;
            case Attribute.Attack: return Level / 6.5f;
            case Attribute.PhysicalDefense: return Level / 25;
            case Attribute.MagicalDefense: return Level / 20;
            case Attribute.Speed: return Level / 100;
        }
        
        return 1;
    }

    private void LoadLevelExperienceMap()
    {
        //TODO -> Use CSVUtils
        PlayerlevelExperienceMap = new Dictionary<int, int>();

        var filePath = Path.Combine(Application.dataPath, "Resources/CSV/PlayerXpCSVExport.csv");

        var csvLines = File.ReadAllLines(filePath);

        foreach (var line in csvLines)
        {
            var values = line.Split(',');

            if (values.Length >= 2)
            {
                var level = int.Parse(values[0]);
                var experienceRequired = int.Parse(values[1]);

                PlayerlevelExperienceMap[level] = experienceRequired; // Associe le niveau à l'expérience requise
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }
    }
    public void GainExperiencePlayer(int expToAdd)
    {
        Level = PlayFabManager.Instance.Player.Level;
        Exp = PlayFabManager.Instance.Player.Exp;

        if (Level >= PlayerlevelExperienceMap.Count)
            // Le joueur a atteint le niveau maximum, ne gagne plus d'expérience
            return;

        Exp += expToAdd; // Ajoute l'expérience spécifiée
        Debug.Log("player gain " + expToAdd + " xp");



        while (PlayerlevelExperienceMap.ContainsKey(Level + 1) && Exp >= PlayerlevelExperienceMap[Level + 1])
        {
            Level++; // Incrémente le niveau
            Exp -=
                PlayerlevelExperienceMap[Level]; // Déduit l'expérience requise pour atteindre le niveau suivant
            PlayFabManager.Instance.Player.Level = Level;
            PlayFabManager.Instance.Player.Exp = Exp;
            OnPlayerLevelUp?.Invoke(Level, LevelUpQuestEvent.LvlType.Player);
        }
        Debug.Log("player xp " + Exp);
        Debug.Log("Level " + Level);
        PlayFabManager.Instance.Player.Exp = Exp;
        PlayFabManager.Instance.UpdateData();

        //UpdateUI(); // Met à jour l'interface utilisateur
    }
}