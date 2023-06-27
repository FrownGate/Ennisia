using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : ElementsBuff
{
    public override void BuffElement(Entity _player)
    {
        if (_Supportelements[0] == Element.Earth.ToString())
        {
            _player.Stats[Item.AttributeStat.MagicalDefense].AddModifier(Buff);
            _player.Stats[Item.AttributeStat.PhysicalDefense].AddModifier(Buff);
        }
    }

    private float Buff(float value)
    {
        return value * 0.1f;
    }
}
