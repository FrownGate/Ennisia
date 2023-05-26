using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderousImpact : Skill
{


    private void Awake()
    {
        fileName = "ThunderousImpact";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //TODO -> stun for 1 turn
        return 0;
    }
}
