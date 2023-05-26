using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllIn : Skill
{
    private void Awake()
    {
        fileName = "AllIn";
    }

    public override void ConstantPassive(Entity target, Entity player, int turn)
    {
        player.Speed = 10000;
        float attackBuff = player.Defense * 0.5f;
        player.Damage = attackBuff;
    }

}