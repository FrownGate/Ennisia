using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purifyingWard : Skill
{
    private void Awake()
    {
        fileName = "purifyingWard";
    }

    public override void PassiveAfterAttack(Entity target, Entity player, int turn, float damage)
    {
        // to do : cleanse one debuff every turn
    }

}
