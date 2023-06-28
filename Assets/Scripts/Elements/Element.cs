using System;
using UnityEngine;

public class Element : MonoBehaviour
{
    public enum ElementType
    {
        Fire,
        Water,
        Wind,
        Earth,
        Thunder
    }

    private ElementType[] _supportsElement;

    private void Start()
    {
        _supportsElement = new ElementType[PlayFabManager.Instance.Player.EquippedSupports.Length];

        for (int i = 0; i < _supportsElement.Length; i++)
        {
            SupportCharacterSO support = PlayFabManager.Instance.Player.EquippedSupports[i];

            if (support == null) continue;
            _supportsElement[i] = support.Element;
        }
    }

    public void CheckElements(Entity _player)
    {
        if (_supportsElement[0] != _supportsElement[1]) return;
        Type type = Type.GetType(_supportsElement[0].ToString());
        Element elementToUse = (Element)Activator.CreateInstance(type);
        elementToUse.BuffElement(_player);
    }

    protected virtual void BuffElement(Entity _player) { }
}