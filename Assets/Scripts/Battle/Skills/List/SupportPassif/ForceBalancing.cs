using System.Collections.Generic;

public class ForceBalancing : PassiveSkill
{
    private bool _hasBuff = false;
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            if (target.Effects.Count > 0 && !_hasBuff)
            {
                _hasBuff = true;
            }
            else continue;
        }

        if (_hasBuff)
        {
            caster.ApplyEffect(new AttackBuff());
            caster.ApplyEffect(new DefenseBuff());
        }
    }
}