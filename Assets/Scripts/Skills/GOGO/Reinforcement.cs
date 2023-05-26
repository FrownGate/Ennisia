using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforcement : Skill
{
    private void Awake()
    {
        fileName = "Reinforcement";
    }

    public override void ConstantPassive(Entity target, Entity player, int turn)
    {
        float defBuff = player.Defense * 0.05f;
        player.Damage = defBuff;
    }

}
