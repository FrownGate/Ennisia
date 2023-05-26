using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazingFury : Skill
{

    int _increaseAttTurn;
    private void Awake()
    {
        fileName = "BlazingFury";
    }
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        if (turn <= _increaseAttTurn)
        {
            //TODO -> increase damage  

        }

    }
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        _increaseAttTurn = turn + 3;
        return 0;
    }
}
