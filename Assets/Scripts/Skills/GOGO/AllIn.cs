using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllIn : Skill
{
    private void Awake()
    {
        fileName = "AllIn";
    }

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        player.Speed = 10000;
        float attackBuff = player.PhysDef * 0.5f + player.MagicDef * 0.5f;
        player.Attack = attackBuff;
    }

}