using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaleDisruption : Skill
{


    private void Awake()
    {
        fileName = "GaleDisruption";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //TODO -> cleanse buffs of target
        return 0;
    }
}
