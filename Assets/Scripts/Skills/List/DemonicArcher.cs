using System.Collections.Generic;

public class DemonicArcher : PassiveSkill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.MagicalDefense].AddModifier(DefenseBuff);
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(DefenseBuff);
    }

    float DefenseBuff(float value) => (float)value * 1.20f;
}