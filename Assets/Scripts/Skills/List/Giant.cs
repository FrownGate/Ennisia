using System.Collections.Generic;

public class Giant : PassiveSkill
{
    private float _healthBaseRatio;
    private float _healthRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _healthRatio = _healthBaseRatio + StatUpgrade1 * Level;
        _modifiers[Attribute.HP] = caster.Stats[Attribute.HP].AddModifier(MaxHPBuff);
    }

    float MaxHPBuff(float input) => input * _healthRatio;
}