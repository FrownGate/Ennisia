using System.Collections.Generic;

public class Swiftness : PassiveSkill
{
    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.Speed] = caster.Stats[Attribute.Speed].AddModifier(Speed);
    }

    float Speed(float value) => ((Data.BuffAmount / 100) + StatUpgrade1 * Level) * value;
}