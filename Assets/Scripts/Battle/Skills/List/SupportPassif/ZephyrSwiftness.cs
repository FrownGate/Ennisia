using System.Collections.Generic;

public class ZephyrSwiftness : PassiveSkill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        _modifiers[Attribute.Speed] = caster.Stats[Attribute.Speed].AddModifier(SpeedModifier);
    }

    float SpeedModifier(float value) => value + (value * Data.BuffAmount / 100);
}