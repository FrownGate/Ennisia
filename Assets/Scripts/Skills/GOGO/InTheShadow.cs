using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTheShadow : Skill
{
    private void Awake()
    {
        fileName = "InTheShadow";
    }

    public override float Use(Entity target, Entity player, int turn)
    {
        //if enemy is debuff, 50% chance to play again
        return 0;
    }
}
