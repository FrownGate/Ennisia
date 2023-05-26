using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HolyPrayer : Skill
{

    private void Awake()
    {
        fileName = "HolyPrayer";
    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //attacksBuff

        player.CurrentHp += player.MaxHp * 10 / 100;
        cd = data.maxCooldown;
        return 0;
    }

}
    
