using UnityEngine;

public class Player : Entity
{
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

    //public Player(Dictionary<string, float> stats)
    //{
    //    stats = new Dictionary<string, float>()
    //    {
    //        //TODO -> use stat enum instead of strings
    //        { "MaxHp", (int)MaxHp },
    //        { "Atk", (int)Attack },
    //        { "PhysAtk", (int)PhysAtk },
    //        { "PhysDef", (int)PhysDef },
    //        { "MagicAtk", (int)MagicAtk },
    //        { "MagicDef", (int)MagicDef },
    //        { "CritRate", (int)CritRate },
    //        { "CritDamage", (int)CritDamage },
    //        { "Speed", (int)Speed },
    //    };
    //}

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