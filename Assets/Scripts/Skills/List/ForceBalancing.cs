using System.Collections.Generic;

public class ForceBalancing : PassiveSkill
{
    private List<Effect> _debuffList;
    private bool _hasBuff = false;
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        for (int i = 1; i < targets.Count; i++)
        {
            if (targets[i].Effects.Count > 1 && !_hasBuff)
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