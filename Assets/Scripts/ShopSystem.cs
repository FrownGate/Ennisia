using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using Unity.VisualScripting;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject _ShopBtnPanel;
    [SerializeField] private GameObject _ShopItemBtnPanel;
    [SerializeField] private GameObject _shopItemBtnPrefab;
    [SerializeField] private GameObject _shopBtnPrefab;



    private Dictionary<string, PlayFab.EconomyModels.CatalogItem> _shops;
    private int _itemId;

    //private Dictionary<string, List<GameObject>> _shopItems = new ();
    private List<GameObject> _currentShopItems = new List<GameObject>();
    private string _currentShop;

    private Dictionary<Item, int> Items = new();


    void Start()
    {
        _shops = PlayFabManager.Instance.Stores;
        InitShops();
        foreach (var store in PlayFabManager.Instance.Stores)
        {
            foreach (var item in store.Value.ItemReferences)
            {

                Buy(PlayFabManager.Instance.GetItemById(item.Id));
            }
        }
    }

    private void InitShops()
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

    private void InitShopItems(List<CatalogItemReference> itemReferences)
    {
        _currentShopItems.Clear();

        for (int i = 0; i < itemReferences.Count; i++)
        {
            GameObject shopItemBtn = Instantiate(_shopItemBtnPrefab, transform);
            shopItemBtn.transform.SetParent(_ShopItemBtnPanel.transform);

            var itemInfo = shopItemBtn.GetComponent<ShowShopBtnInfo>();
            PlayFab.EconomyModels.CatalogItem item = PlayFabManager.Instance.GetItemById(itemReferences[i].Id);
            itemInfo.ItemName = item.AlternateIds[0].Value;
            itemInfo.ItemPrice = item.PriceOptions.Prices[0].Amounts[0].Amount;
            itemInfo.ItemSprite = Resources.Load<Sprite>("Sprites/Items/" + itemInfo.ItemName);
            Debug.Log(itemInfo.ItemName + "Added");
            _currentShopItems.Add(shopItemBtn);
        }
    }

    public void Buy(PlayFab.EconomyModels.CatalogItem item)
    {
        Item itemToBuy = JsonUtility.FromJson<Bundle>(item.DisplayProperties.ToString());
        switch (item.Type)
        {
            case "SummonTicket":
                itemToBuy = JsonUtility.FromJson<SummonTicket>(item.DisplayProperties.ToString());
                break;
            case "Gear":
                // itemToBuy = JsonUtility.FromJson<Bundle>(item.DisplayProperties.ToString());
                break;
            case "Bundle":
                itemToBuy = JsonUtility.FromJson<Bundle>(item.DisplayProperties.ToString());
                break;
        }
        itemToBuy.IdString = item.Id;
        itemToBuy.Name = item.AlternateIds[0].Value;
        itemToBuy.Price = item.PriceOptions.Prices[0].Amounts[0].Amount;

        Debug.LogWarning(itemToBuy.Available);
        Debug.LogWarning(item.DisplayProperties.ToString());




        PlayFabManager.Instance.PurchaseInventoryItem(itemToBuy);
    }


    public void OnShopBtnClick(string shopName)
    {
        DestroyOnClear();
        foreach (var shop in _shops)
        {
            if (shop.Value.AlternateIds[0].Value == shopName)
            {
                _currentShop = shopName;
                InitShopItems(shop.Value.ItemReferences);
            }
        }
    }

    private void DestroyOnClear()
    {
        foreach (var item in _currentShopItems)
        {
            Destroy(item);
        }
        _currentShopItems.Clear();
    }
}