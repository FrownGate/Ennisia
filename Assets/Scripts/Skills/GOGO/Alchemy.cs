using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchemy : Skill
{
    private void Awake()
    {
        fileName = "Alchemy";
    }

    public override void ConstantPassive(Entity target, Entity player, int turn)
    {
        float PhRatioBuff = 0.5f;
        player.PhysicalRatio += PhRatioBuff;
    }

    public override void PassiveAfterAttack(Entity target, Entity player, int turn, float damage)
    {
        if(turn %2 == 0)
        {
            //to do : reduce 3rd skill cd by # 
        }
    }

}
