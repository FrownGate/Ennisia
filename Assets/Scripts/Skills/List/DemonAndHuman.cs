using System.Collections.Generic;

public class DemonAndHuman : Skill
{
    private float _magicalBuffRatio;
    private float _physicalBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDamages] = caster.Stats[Attribute.MagicalDamages].AddModifier(MagicalDefBuff);
        _modifiers[Attribute.PhysicalDamages] = caster.Stats[Attribute.PhysicalDamages].AddModifier(PhysicalDefBuff);
    }

    float MagicalDefBuff(float input) => input * (1 + _magicalBuffRatio);
    float PhysicalDefBuff(float input) => input * (1 + _physicalBuffRatio);
}