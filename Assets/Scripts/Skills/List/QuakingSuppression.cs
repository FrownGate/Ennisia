using System.Collections.Generic;
using UnityEngine;

public class QuakingSuppression : Skill
{
    private float _stunPerc = 0.7f;
    private int _silenceTurn;

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        if (turn <= _silenceTurn)
        {
            //TODO -> silence
        }
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float stunLuck = Random.Range(0, 1);

        if (stunLuck > _stunPerc)
        {
            //TODO -> Stun
        }

        _silenceTurn = turn + 2;

        return 0;
    }
}