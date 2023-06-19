using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class Fire : ElementsBuff
{
    public override void BuffElement(Entity _player)
    {

        if (_Supportelements[0] == Element.Fire.ToString())
        {
            _player.Stats[Item.AttributeStat.HP].AddModifier(Buff);
        }
    }

    private float Buff(float value)
    {
        return value * 0.15f;
    }
}
