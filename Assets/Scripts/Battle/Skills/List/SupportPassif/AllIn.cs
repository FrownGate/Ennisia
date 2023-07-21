using System.Collections.Generic;

public class AllIn : PassiveSkill
{
    private float _defBaseRatio;
    private float _attackBuff;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        caster.AtkBarPercentage = 100;
        _attackBuff = (caster.Stats[Attribute.PhysicalDefense].Value + caster.Stats[Attribute.MagicalDefense].Value ) * ((Data.BuffAmount / 100) + (StatUpgrade1 * Level));
        _modifiers[Attribute.Attack] = caster.Stats[Attribute.Attack].AddModifier(AttackBuff);
    }

    float AttackBuff(float value) => value + _attackBuff;
}