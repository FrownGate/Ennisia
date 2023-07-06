using System.Collections.Generic;

public class BigMushroom : PassiveSkill
{
    private bool _hasModifier = false;

    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        if (targets.Count <= 2 && !_hasModifier)
        {
            _modifiers[Attribute.Attack] = caster.Stats[Attribute.Attack].AddModifier(AttackBuf);
            _hasModifier = true;
        }
        else if (targets.Count >= 2 && _hasModifier)
        {
            _hasModifier = false;
            TakeOffStats(caster);
        }
    }

    float AttackBuf(float value) => (float)value * 2;

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        caster.Heal(damage);
    }
}