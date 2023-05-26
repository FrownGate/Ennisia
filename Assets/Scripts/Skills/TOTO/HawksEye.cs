using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawksEye : Skill
{


    private void Awake()
    {
        fileName = "HawksEye";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //give CR / CD / ATTACK Buff
        //give additional turn
        return 0;
    }
}

