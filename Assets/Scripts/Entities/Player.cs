using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    //TODO -> Add Gear modifiers

    public Player()
    {
        PlayerData playerData = PlayFabManager.Instance != null ? PlayFabManager.Instance.Player : null;

        Name = playerData != null ? playerData.Name : "PlayerName";
        Level = playerData != null ? playerData.Level : 1;
        Weapon = playerData != null && playerData.EquippedGears[GearType.Weapon] != null
            ? playerData.EquippedGears[GearType.Weapon].WeaponSO
            : Resources.Load<GearSO>("SO/EquippedGears/Weapon");
        Weapon.Init(); //Temp

        //TODO -> Set stats with CSV or another method
        Stats[Attribute.Speed] = new(90); //Temp
        Skills = new List<Skill>
        {
            new Bonk()
        };

        if (Weapon == null) return;
        Skills.Add(Weapon.Skills.FirstOrDefault());
        Skills.Add(Weapon.Skills.Skip(1).FirstOrDefault());
    }
}