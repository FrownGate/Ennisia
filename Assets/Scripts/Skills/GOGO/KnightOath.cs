using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightOath : Skill
{
    private void Awake()
    {
        fileName = "KnightOath";
    }

    public override void ConstantPassive(Entity target, Entity player, int turn)
    {
        float buffMaxHp = player.MaxHp * 0.15f;
        float buffDef = player.Defense * 0.15f;
        player.MaxHp += buffMaxHp;
        player.Defense += buffDef;
    }


    //add revenge : after receiving dmg, give a shield of 5% of max hp :: shieldModifier = player.MaxHp * 0.05f; ;
}
