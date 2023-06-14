using System;
using System.Collections.Generic;
using UnityEngine;
using static Item;

[Serializable]
public class PlayerData
{
    public int Level;
    public int Exp;
    public int[] EquippedGearsId;
    public string[] EquippedSupportsPath;
    [NonSerialized] public SupportCharacterSO[] EquippedSupports;
    [NonSerialized] public Dictionary<GearType, Gear> EquippedGears;

    public PlayerData()
    {
        Level = 1;
        Exp = 0;
        EquippedGearsId = new int[7];
        EquippedGears = new();
        EquippedSupportsPath = new string[2] { null, null };
        EquippedSupports = new SupportCharacterSO[2] { null, null };

        foreach (var item in Enum.GetNames(typeof(GearType)))
        {
            EquippedGears[Enum.Parse<GearType>(item)] = null;
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

    public void Equip(Gear gear)
    {
        EquippedGearsId[(int)gear.Type] = gear.Id;
        EquippedGears[(GearType)gear.Type] = gear;

        if (gear.Type == GearType.Weapon) gear.WeaponSO.Init();

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