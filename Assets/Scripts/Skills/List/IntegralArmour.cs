using System.Collections.Generic;

public class IntegralArmour : Skill
{
    private float _physicalDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(PhysicalDefBuff);
    }

    float PhysicalDefBuff(float value) => value * (1 + _physicalDefBuffRatio);
}