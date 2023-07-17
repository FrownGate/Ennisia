using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject _shopBtnPrefab;


    private Dictionary<string, PlayFab.EconomyModels.CatalogItem> _shops;
    private int _itemId;
    
    void Start()
    {
        _shops = PlayFabManager.Instance.Stores;
    }
    
    void Update()
    {
        
    }

    public void Buy()
    {
        if (PlayFabManager.Instance.HasEnoughCurrency(_itemId))
        {
            //PlayFabManager.Instance.RemoveCurrency();
            //TODO -> Add item to player's inventory
            
        }
        
    }
    
}
