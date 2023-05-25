using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodContract : Skill
{


    private void Awake()
    {
        fileName = "BloodContract";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        return 0;
    }
}

