using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePurification : Skill
{


    private void Awake()
    {
        fileName = "FlamePurification";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //TODO -> cleanse all debuff
        return 0;
    }
}
