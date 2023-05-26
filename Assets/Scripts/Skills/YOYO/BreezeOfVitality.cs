using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreezeOfVitality : Skill
{
    float _increaseHealPerc = 10f;

    private void Awake()
    {
        fileName = "BreezeOfVitality";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float addHeal = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].currentHp > 0)
            {
                addHeal += _increaseHealPerc;
            }
        }
        float heal = player.maxHp * (data.healingAmount + addHeal) / 100;

        player.currentHp += heal;

        return 0;
    }
}
