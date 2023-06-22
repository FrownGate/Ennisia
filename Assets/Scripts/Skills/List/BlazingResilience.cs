using System.Collections.Generic;
using UnityEngine;

public class BlazingResilience : PassiveSkill
{
//TODO -> When the plate receives a debuff, give random buff (att, Crit Rate, Crit Damage).
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) 
    {
        //if player gain debuff
        int nbBuff = 3;
        int randomBuff = Random.Range(0, nbBuff);
        switch (randomBuff)
        {
            case 0:
                //give Buff att
                break;
            case 1:
                //give Buff Crit Rate
                break;
            case 2:
                //give Buff Crit Damage
                break;
            default:
                break;
        }
    }

}
