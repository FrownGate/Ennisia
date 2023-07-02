using System.Collections.Generic;

public class Revitalise : Skill
{
    public float _hpBuffRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.HP] = caster.Stats[Attribute.HP].AddModifier(HpBuff);
    }

    float HpBuff(float value) => value * (1 + _hpBuffRatio);
}