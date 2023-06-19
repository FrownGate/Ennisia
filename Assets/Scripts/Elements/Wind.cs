using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : ElementsBuff
{
    public override void BuffElement(Entity _player)
    {
        if (_Supportelements[0] == Element.Wind.ToString())
        {
            _player.Stats[Item.AttributeStat.CritDmg].AddModifier(Buff);
        }
    }

    private float Buff(float value)
    {
        return value + 20;
    }
}
