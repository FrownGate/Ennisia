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
}