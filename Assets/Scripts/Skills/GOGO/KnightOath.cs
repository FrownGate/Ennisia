using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightOath : Skill
{
    private void Awake()
    {
        fileName = "KnightOath";
    }

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float buffMaxHp = player.MaxHp * 0.15f;
        float buffPhDef = player.PhysDef * 0.15f;
        float buffMDef = player.MagicDef * 0.15f;
        player.MaxHp += buffMaxHp;
        player.PhysDef += buffPhDef;
        player.MagicDef += buffMDef;
    }


    //add revenge : after receiving dmg, give a shield of 5% of max hp :: shieldModifier = player.MaxHp * 0.05f; ;
}
