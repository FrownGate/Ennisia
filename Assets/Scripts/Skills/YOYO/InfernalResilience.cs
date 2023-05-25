using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernalResilience : Skill
{

    private void Awake()
    {
        fileName = "RampantAssault";
    }
    public override void Use(Entity target, Entity player, int turn)
    {
        float missingHealth = player.maxHp - player.currentHp;
        //give shield for 3 turn for shieldamount
    }
}
