using System.Collections.Generic;

public class AccuracyDevice : Skill
{
    private float _critRateBuffRatio;
    private float _critDmgBuffRatio;
    private float _attackBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.CritRate] = caster.Stats[Attribute.CritRate].AddModifier(CritRateBuff);
        _modifiers[Attribute.CritDmg] = caster.Stats[Attribute.CritDmg].AddModifier(CritDmgBuff);
        _modifiers[Attribute.Attack] = caster.Stats[Attribute.Attack].AddModifier(AttackBuff);
    }

    float CritRateBuff(float input) => input * (1 + _critRateBuffRatio);
    float CritDmgBuff(float input) => input * (1 + _critDmgBuffRatio);
    float AttackBuff(float input) => input * (1 + _attackBuffRatio);
}