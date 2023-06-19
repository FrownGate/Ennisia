using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsBuff : MonoBehaviour
{
    public enum Element
    {
        Fire,Water,Wind,Earth,Thunder
    }

    public string[] _Supportelements;
    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            _Supportelements[i] = PlayFabManager.Instance.Player.EquippedSupports[i].Element;
        }
    }

    public void CheckElements(Entity _player)
    {
        if (_Supportelements[0] == _Supportelements[1])
        {
            Type type = System.Type.GetType(_Supportelements[0]);
            ElementsBuff elementToUse = (ElementsBuff)Activator.CreateInstance(type);
            elementToUse.BuffElement(_player);
        }
    }

    public virtual void BuffElement(Entity _player)
    {
        
    }
}
