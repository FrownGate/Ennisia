using System.Collections.Generic;

public class ImpregnableDefense : PassiveSkill
{
    private int _percentage;

    public ImpregnableDefense()
    {
        _percentage = 0;
    }

    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        if (_percentage < 20)
        {
            _percentage += 5;
            TakeOffStats(caster);
            _modifiers[Attribute.PhysicalDefense] = caster.Stats[Attribute.PhysicalDefense].AddModifier(AddPercentToDef);
            _modifiers[Attribute.MagicalDefense] = caster.Stats[Attribute.MagicalDefense].AddModifier(AddPercentToDef);
        }
    }

    float AddPercentToDef(float value) => value + (_percentage / 100 * value);
}