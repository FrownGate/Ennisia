using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastHope : Skill
{
    bool isUsed;
    private void Awake()
    {
        isUsed = false;
        fileName = "LastHope";
    }

    public override void PassiveAfterAttack(List<Entity> target, Entity player, int turn, float damage)
    {
        if(player.CurrentHp < player.MaxHp * 0.2f & isUsed)
        {
            isUsed = true;
            healingModifier = player.MaxHp * 0.3f;
        }
    }

}