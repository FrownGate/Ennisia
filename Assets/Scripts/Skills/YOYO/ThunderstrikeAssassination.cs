using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstrikeAssassination : Skill
{
    private void Awake()
    {
        fileName = "ThunderstrikeAssassination";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {

        return 0;
    }
}
