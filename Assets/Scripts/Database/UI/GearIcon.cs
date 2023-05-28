using UnityEngine;
using UnityEngine.UI;

public class GearIcon : MonoBehaviour
{
    [SerializeField] private EquipmentSO _equipment;
    [SerializeField] private Image _icon;

    private void Awake()
    {
        _icon.sprite = _equipment.Icon ?? _icon.sprite;
    }
}