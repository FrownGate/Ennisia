using System.Collections.Generic;

public class Alchemy : PassiveSkill
{
    private float _magicAtkBaseRatio;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _modifiers[Attribute.MagicalDamages] = caster.Stats[Attribute.MagicalDamages].AddModifier(MRatioBuff);
    }

    float MRatioBuff(float value) => value * (_magicAtkBaseRatio + StatUpgrade1 * Level);

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        if (turn %2 == 0) caster.Skills[2].Cooldown -= 1;
    }
}