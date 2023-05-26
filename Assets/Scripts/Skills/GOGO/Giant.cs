using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Skill
{
    private void Awake()
    {
        fileName = "Giant";
    }

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float maxHpBuff = player.MaxHp * 0.05f;
        player.Attack = maxHpBuff;
    }

}
