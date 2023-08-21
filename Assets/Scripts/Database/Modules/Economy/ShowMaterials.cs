using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMaterials : MonoBehaviour
{
    private GameObject _materialPrefab;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _materialPrefab = Resources.Load<GameObject>("Prefabs/UI/Material");
        _rectTransform = GetComponent<RectTransform>();

        int position = 0;

        foreach (var item in PlayFabManager.Instance.GetItems())
        {
            if (item is not Material) continue;
            if (item.Category == ItemCategory.Weapon) continue;
            GameObject materialObject = Instantiate(_materialPrefab, gameObject.transform);
            materialObject.transform.localPosition =
                new Vector3((0 - _rectTransform.rect.width / 2.3f) + position, 0, 0);

            TextMeshProUGUI text = materialObject.GetComponentInChildren<TextMeshProUGUI>();
            text.color = item.Rarity switch
            {
                Rarity.Common => Color.grey,
                Rarity.Rare => Color.blue,
                Rarity.Epic => Color.magenta,
                Rarity.Legendary => Color.yellow,
                _ => text.color
            };
            text.text = item.Amount.ToString();

            position += 180;
        }
    }
}