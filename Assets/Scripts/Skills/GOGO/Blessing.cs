using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : Skill
{
    private void Awake()
    {
        fileName = "Blessing";
    }

    public override float Use(Entity target, Entity player, int turn)
    {
        return damageModifier = player.damage * 0.05f;
    }
}
