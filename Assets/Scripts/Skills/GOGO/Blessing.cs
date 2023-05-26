using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : Skill
{
    private void Awake()
    {
        fileName = "Blessing";
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        healingModifier = damage * 0.1f;

        if(turn % 2 == 0)
        {
            // to do : give immunity
        }
    }
}
