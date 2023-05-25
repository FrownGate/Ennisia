using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampantAssault : Skill
{
    private const float _percentagePerTurn = 0.05f;
    private float _damagePercentage = 0.2f;

    
    public override void Use(Entity target, Entity player, int turn)
    {
        damageModifier = target.maxHp * _damagePercentage;
        float percOfAddDamage = _percentagePerTurn * turn;

        if (percOfAddDamage > 0.5f)
        {
            percOfAddDamage = 0.5f;
        }

        float damagePerTurn = damageModifier * percOfAddDamage;
        damageModifier += damagePerTurn;
    }
}
