using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : Skill
{
    private void Awake()
    {
        fileName = "Blessing";
    }

    public override float Use(List<Entity> target, Entity player, int turn)
    {
        return damageModifier = player.PhysAtk * 0.05f;
    }
}
