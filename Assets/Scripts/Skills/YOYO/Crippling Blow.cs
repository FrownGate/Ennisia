using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CripplingBlow : Skill
{

    //replace player.damage

    private void Awake()
    {
        fileName = "CripplingBlow";
    }

    public override float Use(List<Entity> target, Entity player, int turn)
    {
        return 0;
    }
}
