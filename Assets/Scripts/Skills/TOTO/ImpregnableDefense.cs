using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpregnableDefense : Skill
{

    float defenseAdded;
    float defenseToAdd;
    private void Awake()
    {
        fileName = "ImpregnableDefense";
        defenseAdded= 0;
    }

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        //defenseToAdd = player.Def * 5/100
    }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {

        if(turn !>4)
        {
            defenseAdded = defenseToAdd * turn;
        }

        //need to add afterBattleFunction to take off stats
    }

}

