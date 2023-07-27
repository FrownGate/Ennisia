using System.Collections.Generic;

public class ThunderstormAmplification : PassiveSkill
{
    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        _modifiers[Attribute.MagicalDamages] =  caster.Stats[Attribute.MagicalDamages].AddModifier(MagicBuff);
    }

    private float MagicBuff(float value) => value + (value * Data.BuffAmount); 
}