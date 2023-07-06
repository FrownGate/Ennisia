using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    //TODO -> Add Gear modifiers
    private PlayerData Data => PlayFabManager.Instance != null ? PlayFabManager.Instance.Player : null;
    private bool HasData => Data != null;

    public override string Name => HasData ? Data.Name : "PlayerName";
    public override int Level => HasData ? Data.Level : 1;
    public override Dictionary<Attribute, Stat<float>> Stats => HasData ? Data.Stats : DefaultStats();
    public int Exp => HasData ? Data.Exp : 0;

    public SupportCharacterSO[] EquippedSupports => HasData ? Data.EquippedSupports : new SupportCharacterSO[2] { null, null };
    public Dictionary<GearType, Gear> EquippedGears => HasData ? Data.EquippedGears : DefaultGears();

    public override GearSO Weapon => Data != null && EquippedGears[GearType.Weapon] != null
        ? EquippedGears[GearType.Weapon].WeaponSO
        : Resources.Load<GearSO>("SO/EquippedGears/Weapon");

    public Player()
    {
        //TODO -> Set stats with CSV or another method
        Stats[Attribute.Speed] = new(90); //Temp

        Skills = new List<Skill>
        {
            new Bonk()
        };

        Weapon.Init();
        foreach (var skill in Weapon.Skills) Skills.Add(skill);
    }

    private Dictionary<GearType, Gear> DefaultGears()
    {
        Dictionary<GearType, Gear> gears = new();

        foreach (var item in Enum.GetNames(typeof(GearType)))
        {
            gears[Enum.Parse<GearType>(item)] = null;
        }

        return gears;
    }
}