using System.Collections.Generic;
using System.Data;

public class GustofPrecision : PassiveSkill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        _modifiers[Attribute.CritRate] = caster.Stats[Attribute.CritRate].AddModifier(AddCritRate);
    }

    float AddCritRate(float value) => value + Data.BuffAmount;
}