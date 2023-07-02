using UnityEngine;

public class Player : Entity
{
    //TODO -> Add Gear modifiers

    public Player()
    {
        //TODO -> Set stats with CSV or another method
        /*     MaxHp = 300;
             Attack = 15;
             Speed = 30000;*/
        Name = "heho";
        CurrentHp = Stats[Attribute.HP].Value;
        Stats[Attribute.Speed] = new(90);
        Weapon = Resources.Load<GearSO>("SO/EquippedGears/Weapon"); //Temp
        //Weapon = PlayFabManager.Instance.Player.EquippedGears[Item.GearType.Weapon].WeaponSO;
        Weapon.Init();

        InitSkills();
    }

    private void InitSkills()
    {
        Skills = new()
        {
            new Bonk(),
            Weapon.FirstSkill,
            Weapon.SecondSkill
        };
    }
}