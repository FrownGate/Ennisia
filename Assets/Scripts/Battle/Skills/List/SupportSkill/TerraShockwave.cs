using System.Collections.Generic;
using UnityEngine;

public class TerraShockwave: DamageSkill
{
    private float _stunPerc = 0.5f;
    private float _increaseCDPerc = 0.75f;
    private int _stunTurn;

    public override void ConstantPassive(List<Entity> targets, Entity caster, int turn)
    {
        if (turn <= _stunTurn)
        {
            //TODO -> stun   
        }
    }

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        DamageModifier = caster.Stats[Attribute.PhysicalDamages].Value * StatUpgrade1 * Level;
        targets[0].TakeDamage(DamageModifier);
        float stunLuck = Random.Range(0, 1);

        if (stunLuck > _stunPerc) _stunTurn = turn + 1;

        float increaseCDLuck = Random.Range(0, 1);

        if (increaseCDLuck > _increaseCDPerc)
        {
            //TODO -> targets[0].cd += 2;
        }

        return 0;
    }
}