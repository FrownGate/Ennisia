using System.Collections.Generic;

public class MakeOrBreak : PassiveSkill
{

    private float _maxHpBuff;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _maxHpBuff = caster.Stats[Attribute.Attack].Value * ((Data.BuffAmount/100) + StatUpgrade1 * Level);
        _modifiers[Attribute.HP] = caster.Stats[Attribute.HP].AddModifier(AttackBuff);
    }

    float AttackBuff(float input) => input + _maxHpBuff;

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        caster.Heal(damage * ((Data.HealingAmount/100) + StatUpgrade2 * Level));
    }
}