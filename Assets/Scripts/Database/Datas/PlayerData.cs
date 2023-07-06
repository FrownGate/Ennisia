using System;
using System.Collections.Generic;
using UnityEngine;

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
    [NonSerialized] public readonly Dictionary<Attribute, Stat<float>> Stats = new(); //Serialize ?

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
        }

        foreach (string stat in Enum.GetNames(typeof(Attribute)))
        {
            Stats[Enum.Parse<Attribute>(stat)] = new(1);
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
        EquippedGearsId[(int)gear.Type] = gear.Id;
        EquippedGears[(GearType)gear.Type] = gear;

        if (gear.Type == GearType.Weapon) gear.WeaponSO.Init();
    }

    public void Equip(Gear gear)
    {
        EquipGear(gear);
        PlayFabManager.Instance.UpdateData();
    }

    public void Equip(List<Gear> gears)
    {
        foreach (Gear gear in gears)
        {
            EquipGear(gear);
        }

        PlayFabManager.Instance.UpdateData();
    }

    public void Equip(SupportCharacterSO support, int slot = 0)
    {
        if (slot < 0 || slot > 1)
        {
            Debug.LogError("Support slot is invalid (must be 0 or 1).");
            return;
        }

        EquippedSupportsPath[slot] = $"SO/SupportsCharacter/{support.Rarity}/{support.name}";
        EquippedSupports[slot] = support;
        EquippedSupports[slot].Init();
        PlayFabManager.Instance.UpdateData();
    }

    public void Unequip(GearType type)
    {
        EquippedGearsId[(int)type] = 0;
        EquippedGears[type] = null;
        PlayFabManager.Instance.UpdateData();
    }

    public void Unequip(int slot)
    {
        if (slot < 0 || slot > 1)
        {
            Debug.LogError("Support slot is invalid (must be 0 or 1).");
            return;
        }

        EquippedSupportsPath[slot] = null;
        EquippedSupports[slot] = null;
        PlayFabManager.Instance.UpdateData();
    }
}