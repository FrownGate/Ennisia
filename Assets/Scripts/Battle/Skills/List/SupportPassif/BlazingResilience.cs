using System.Collections.Generic;
using UnityEngine;

public class BlazingResilience : PassiveSkill
{
    //TODO -> When the player receives a debuff, give random buff (att, Crit Rate, Crit Damage).
    public override void UseIfAttacked(List<Entity> targets, Entity caster, Entity player, int turn, float damageTaken)
    {
        //if player gain debuff
        int nbBuff = 3;
        int randomBuff = Random.Range(0, nbBuff);

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