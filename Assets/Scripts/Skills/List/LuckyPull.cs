using System.Collections.Generic;

public class LuckyPull : Skill
{
    private float _criticalRateBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.CritRate] = caster.Stats[Attribute.CritRate].AddModifier(CritRateBuff);
    }

    float CritRateBuff(float input) => input + _criticalRateBuffRatio;
}