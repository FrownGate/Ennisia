using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class InfernalResilience : Skill
{

    private void Awake()
    {
        fileName = "RampantAssault";
    }
    public override float Use(List<Entity> target, Entity player, int turn)
    {
        float missingHealth = player.MaxHp - player.CurrentHp;
        float shield = missingHealth * data.shieldAmount / 100;
        //give shield for 3 turn for shieldamount
        return 0;
    }
}
