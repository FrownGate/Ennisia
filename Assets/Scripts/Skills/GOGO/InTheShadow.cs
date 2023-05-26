using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTheShadow : Skill
{
    private void Awake()
    {
        fileName = "InTheShadow";
    }

    public override void ConstantPassive(Entity target, Entity player, int turn)
    {
        float PenDefBuff = 0.4f;
        player.PenetrationDefense += PenDefBuff;
    }

     // to do : if enemy is debuff, #% chance to play again
}
