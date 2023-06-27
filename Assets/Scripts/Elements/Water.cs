using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : ElementsBuff
{
    public override void BuffElement(Entity _player)
    {
        if (_Supportelements[0] == Element.Water.ToString())
        {
            _player.Stats[Item.AttributeStat.CritRate].AddModifier(Buff);
        }
    }

    private float Buff(float value)
    {
        return value + 10;
    }
}

