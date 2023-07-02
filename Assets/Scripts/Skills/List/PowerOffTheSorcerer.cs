using System.Collections.Generic;

public class PowerOffTheSorcerer : Skill
{
    private float _magicBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDamages] = caster.Stats[Attribute.MagicalDamages].AddModifier(MagicBuff);
    }

    float MagicBuff(float value) => value * (1 + _magicBuffRatio);
}