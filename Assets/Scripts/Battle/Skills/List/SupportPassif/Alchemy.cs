using System.Collections.Generic;

public class Alchemy : PassiveSkill
{


    public override void ConstantPassive(List<Entity> target, Entity caster, int turn, List<Entity> allies)
    {
        _modifiers[Attribute.MagicalDamages] = caster.Stats[Attribute.MagicalDamages].AddModifier(MRatioBuff);
    }

    float MRatioBuff(float value) => value * ((Data.BuffAmount / 100) + (StatUpgrade1 * Level));

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage,
        List<Entity> allies)
    {
        if (turn % 2 == 0) caster.Skills[2].Cooldown -= 1;
    }
}