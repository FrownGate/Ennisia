using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Skill
{
    private void Awake()
    {
        fileName = "Charge";
    }

    public override void ConstantPassive(Entity target, Entity player, int turn)
    {
        float attackBuff = player.Damage * 0.05f;
        player.Damage = attackBuff;
    }

}
