using System.Collections.Generic;

public class GustofPrecision : PassiveSkill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        _modifiers[Attribute.CritRate] = caster.Stats[Attribute.CritRate].AddModifier(AddCritRate);
    }

    float AddCritRate(float value) => value + 25;
}