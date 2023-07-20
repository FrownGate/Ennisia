using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject _ShopBtnPanel;
    [SerializeField] private GameObject _ShopItemBtnPanel;
    [SerializeField] private GameObject _shopItemBtnPrefab;
    [SerializeField] private GameObject _shopBtnPrefab;
    
    

    private Dictionary<string, PlayFab.EconomyModels.CatalogItem> _shops;
    private int _itemId;
    
    private Dictionary<string, List<GameObject>> _shopItems = new ();
    private string _currentShop;
    

    void Start()
    {
        _shops = PlayFabManager.Instance.Stores;
        InitShops();
    }

    public void InitShops()
    {
        foreach (var store in PlayFabManager.Instance.Stores)
        {
            GameObject shopBtn = Instantiate(_shopBtnPrefab, transform);
            shopBtn.transform.SetParent(_ShopBtnPanel.transform);
            var shopBtnInfo = shopBtn.GetComponent<ShowShopInfo>();
            shopBtnInfo.ShopName = store.Value.AlternateIds[0].Value;
            
            InitShopItems(store.Value.ItemReferences);
        }
    }

    public void InitShopItems(List<CatalogItemReference> itemReferences)
    {
        for (int i = 0; i < itemReferences.Count; i++)
        {
            GameObject shopItemBtn = Instantiate(_shopItemBtnPrefab, transform);
            shopItemBtn.transform.SetParent(_ShopItemBtnPanel.transform);
            
            var itemInfo = shopItemBtn.GetComponent<ShowShopBtnInfo>();
            PlayFab.EconomyModels.CatalogItem item = PlayFabManager.Instance.GetItemById(itemReferences[i].Id);
            itemInfo.ItemName = item.AlternateIds[0].Value;
            itemInfo.ItemPrice = item.PriceOptions.Prices[0].Amounts[0].Amount;
        }
    }

    public void Buy(PlayFab.EconomyModels.CatalogItem item)
    {
        Item newItem = Item.CreateFromCatalogItem(item);
        PlayFabManager.Instance.PurchaseInventoryItem(newItem);
    }
}