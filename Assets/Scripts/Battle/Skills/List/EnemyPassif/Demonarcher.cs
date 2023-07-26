using System.Collections.Generic;

public class DemonArcher : Skill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.MagicalDefense].AddModifier(BuffDef);
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(BuffDef);
    }

    private float BuffDef(float value) => value + (value * Data.BuffAmount);

}
