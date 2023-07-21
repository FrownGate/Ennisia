using System.Collections.Generic;

public class Giant : PassiveSkill
{


    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {

        _modifiers[Attribute.HP] = caster.Stats[Attribute.HP].AddModifier(MaxHPBuff);
    }

    float MaxHPBuff(float input) => input * ((Data.BuffAmount / 100) + StatUpgrade1 * Level);
}