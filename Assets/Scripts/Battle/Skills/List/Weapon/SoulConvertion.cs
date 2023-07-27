using System.Collections.Generic;

public class SoulConvertion : PassiveSkill
{
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn, List<Entity> allies)
    {
        bool weaponDmgType = !caster.Weapon.IsMagic;
        float weaponDmgBuff = caster.Weapon.StatValue * ((Data.BuffAmount/100) + (StatUpgrade1 * Level));
        caster.Weapon.IsMagic = weaponDmgType;
        caster.Weapon.StatValue += weaponDmgBuff;
    }
}