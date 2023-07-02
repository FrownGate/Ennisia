using System.Collections.Generic;

public class Herald : Skill
{
    private float _speedBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.Speed] = caster.Stats[Attribute.Speed].AddModifier(SpeedBuff);
    }

    float SpeedBuff(float value) => value * (1 + _speedBuffRatio);
}