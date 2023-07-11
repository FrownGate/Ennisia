using System.Collections.Generic;

public class MakeOrBreak : PassiveSkill
{
    private float _maxHpBaseRatio;
    private float _healOnDmg;
    private float _maxHpBuff;

    public override void ConstantPassive(List<Entity> target, Entity caster, int turn)
    {
        _maxHpBuff = caster.Stats[Attribute.Attack].Value * (_maxHpBaseRatio + StatUpgrade1 * Level);
        _modifiers[Attribute.Attack] = caster.Stats[Attribute.Attack].AddModifier(AttackBuff);
    }

    float AttackBuff(float input) => (float)input + _maxHpBuff;

    public override void PassiveAfterAttack(List<Entity> target, Entity caster, int turn, float damage)
    {
        _healOnDmg = 0.03f + StatUpgrade2 * Level;
        HealingModifier = damage * _healOnDmg;
    }
}