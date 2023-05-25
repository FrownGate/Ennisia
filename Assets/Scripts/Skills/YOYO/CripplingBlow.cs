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

    public override void Use(Entity target, Entity player, int turn)
    {
        damageModifier = player.damage * data.damageAmount;
    }
}
