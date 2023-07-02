using System.Collections.Generic;

public class Executioner : Skill
{
    private float _criticalDmgBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.CritDmg] = caster.Stats[Attribute.CritDmg].AddModifier(CritDmgBuff);
    }

    float CritDmgBuff(float value) => value + _criticalDmgBuffRatio;
}