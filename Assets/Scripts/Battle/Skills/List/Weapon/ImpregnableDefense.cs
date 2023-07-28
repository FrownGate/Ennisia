using System.Collections.Generic;

public class ImpregnableDefense : PassiveSkill
{
    private float _percentage;

    public ImpregnableDefense()
    {
        _percentage = 0;
    }

    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        if (_percentage < ((Data.BuffAmount/100) * 4))
        {
            _percentage += (Data.BuffAmount/100);
            _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(AddPercentToDef);
            _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.MagicalDefense].AddModifier(AddPercentToDef);
        }
    }

    float AddPercentToDef(float value) => value * (1 + (_percentage / 100));
}