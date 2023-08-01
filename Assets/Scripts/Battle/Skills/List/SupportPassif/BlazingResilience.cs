using System.Collections.Generic;
using UnityEngine;

public class BlazingResilience : PassiveSkill
{

    public override void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken,
        List<Entity> allies)
    {
        bool _Debuffed = false;
        foreach (Effect effect in caster.Effects)
        {
            if (!effect.HasAlteration) // is it the right way ?
            {
                _Debuffed = true;
            }
        }
 
        if (_Debuffed)
        {
            int randomBuff = Random.Range(0, 3);

            switch (randomBuff)
            {
                case 0:
                    caster.ApplyEffect(new AttackBuff());
                    break;
                case 1:
                    caster.ApplyEffect(new CritRateBuff());
                    break;
                case 2:
                    caster.ApplyEffect(new CritDmgBuff());
                    break;
                default:
                    break;
            }
        }
        
    }
}