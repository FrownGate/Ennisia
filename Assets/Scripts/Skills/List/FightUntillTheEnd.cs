using System.Collections.Generic;

public class FightUntilTheEnd : Skill
{
    private float _physicalBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.PhysicalDamages] = caster.Stats[Attribute.PhysicalDamages].AddModifier(PhysicalBuff);
    }

    float PhysicalBuff(float value) => value * (1 + _physicalBuffRatio);
}