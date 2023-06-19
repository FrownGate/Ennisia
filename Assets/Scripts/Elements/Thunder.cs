using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : ElementsBuff
{
    public override void BuffElement(Entity _player)
    {
        if (_Supportelements[0] == Element.Thunder.ToString())
        {
            _player.Stats[Item.AttributeStat.Attack].AddModifier(Buff);
        }
    }

    private float Buff(float value)
    {
        return value * 0.1f;
    }

}

