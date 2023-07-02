using System.Collections.Generic;

public class ArmorOfTheDeads : Skill
{
    private float _magicalDefBuffRatio;
    private float _physicalDefBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDamages] = caster.Stats[Attribute.MagicalDamages].AddModifier(MagicalDefBuff);
        _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(PhysicalDefBuff);
    }

    float MagicalDefBuff(float value) => value * (1 + _magicalDefBuffRatio);
    float PhysicalDefBuff(float value) => value * (1 + _physicalDefBuffRatio);
}