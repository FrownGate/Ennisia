using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Skill
{
    private void Awake()
    {
        fileName = "Charge";
    }

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float attackBuff = player.Attack * 0.05f;
        player.Attack = attackBuff;
    }

}
