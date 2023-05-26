using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurturingEarthbound : Skill
{


    private void Awake()
    {
        fileName = "NurturingEarthbound";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float lostHealt = player.MaxHp - player.CurrentHp;
        player.CurrentHp += lostHealt * data.healingAmount / 100;
        return 0;
    }
}
