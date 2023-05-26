using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraShockwave: Skill
{
    float _stunPerc = 0.5f;
    float _increaseCDPerc = 0.75f;
    int _stunTurn;
    private void Awake()
    {
        fileName = "TerraShockwave";
    }
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        if (turn <= _stunTurn)
        {
            //TODO -> stun   

        }
        

    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = player.MagicAtk * data.damageAmount / 100;
        targets[0].TakeDamage(damage);
        float stunLuck = Random.Range(0, 1);
        if (stunLuck > _stunPerc)
        {
            _stunTurn = turn + 1;
        }
        float increaseCDLuck = Random.Range(0, 1);
        if (increaseCDLuck > _increaseCDPerc)
        {
            //TODO -> targets[0].cd += 2;
        }
        return damage;
    }

}
