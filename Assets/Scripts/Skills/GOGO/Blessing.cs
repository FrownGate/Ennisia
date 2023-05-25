using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : Skill
{
    private void Awake()
    {
        fileName = "Blessing";
    }

    public override void Use(Entity target, Entity player, int turn)
    {
        damageModifier = player.damage * 0.05f;
    }
}
