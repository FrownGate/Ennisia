using System.Collections.Generic;

public class SoulConvertion : PassiveSkill
{
    private float _dmgBaseRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        bool weaponDmgType = !caster.Weapon.IsMagic;
        float weaponDmgBuff = caster.Weapon.StatValue * (_dmgBaseRatio + StatUpgrade1 * Level);
        caster.Weapon.IsMagic = weaponDmgType;
        caster.Weapon.StatValue += weaponDmgBuff;
    }
}