using UnityEngine;
using UnityEngine.UI;

public class GearIcon : MonoBehaviour
{
    [SerializeField] private EquipmentSO _equipment;
    [SerializeField] private Image _icon;

    private void Awake()
    {
        int equippedId = PlayFabManager.Instance.Player.EquippedGears[_equipment.TypeIndex()];

        if (equippedId == 0) return;
        PlayFabManager.Instance.SetGearData(_equipment, equippedId);
        _icon.sprite = _equipment.Icon ?? _icon.sprite;
    }
}