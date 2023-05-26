using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBalancing : Skill
{
    private void Awake()
    {
        fileName = "ForceBalancing";
    }

    public override void PassiveAfterAttack(Entity target, Entity player, int turn, float damage)
    {
        
    }

}
