using System.Collections.Generic;

public class HeavenlyArmor : PassiveSkill
{
//TODO -> Each turn increases his defense by 2.5%.
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(AddDefense);
        _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.MagicalDefense].AddModifier(AddDefense);

    }
    float AddDefense(float value) => value + Data.BuffAmount;
}
