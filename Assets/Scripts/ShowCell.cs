using UnityEngine;
using UnityEngine.UI;

public class ShowCell : MonoBehaviour
{
    [SerializeField] private GameObject _itemHUD;
    [SerializeField] private GridLayoutGroup _layout;

    private void Awake()
    {

        for (int i = 0; i < 10; i++)
        {
            Instantiate(_itemHUD, _layout.transform);
        }

        foreach (var item in PlayFabManager.Instance.GetItems())
        {
            item.IsSelected = false;
        }
    }
}