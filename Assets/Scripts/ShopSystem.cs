using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private GameObject _shopItemBtnPrefab;
    [SerializeField] private GameObject _shopBtnPrefab;

    private Dictionary<string, PlayFab.EconomyModels.CatalogItem> _shops;
    private string _itemId;

    void Start()
    {
        _shops = PlayFabManager.Instance.Stores;
    }

    void Update()
    {

    }

    public void InitShops()
    {
        foreach (var store in PlayFabManager.Instance.Stores)
        {
            GameObject shopBtn = Instantiate(_shopBtnPrefab, transform);
            var shopBtnInfo = shopBtn.GetComponent<ShowShopInfo>();
            shopBtnInfo.ShopName = store.Value.Id;
            foreach (var item in store.Value.ItemReferences)
            {

            }
        }
    }

    public void InitShopItems(CatalogItemReference itemReference)
    {
        GameObject shopItemBtn = Instantiate(_shopItemBtnPrefab, transform);
        var itemInfo = shopItemBtn.GetComponent<ShowShopBtnInfo>();
        //PlayFab.EconomyModels.CatalogItem item = PlayFabManager.Instance.GetItemById(itemReference.id);
        //itemInfo.ItemName = item.AlternateId[0].Value;
        //itemInfo.Price = item.PriceOptions.Price[0].Amounts[0].Amount;
    }

    public void Buy(PlayFab.EconomyModels.CatalogItem item)
    {
        //TODO -> Add item to player's inventory
        //TODO -> Remove currency from player's inventory

        // Item newItem = JsonUtility.FromJson<Item>(item.DisplayProperties.ToString());
        // newItem.Deserialize();
        Item newItem = Item.CreateFromCatalogItem(item);

        PlayFabManager.Instance.PurchaseInventoryItem(newItem);
        // Type type = Type.GetType(item.AlternateIds[0].Value);
        // Item itemToBuy = new();
        // Activator.CreateInstance(type, itemToBuy);

        // PlayFabManager.Instance.PurchaseInventoryItem(itemToBuy);


        // switch (item.ContentType)
        // {
        //     case "Gear":
        // Gear gear = new(item);
        //         Activator.CreateInstance(Type.GetType(item.Id), gear);
        //         break;
        //     case "Material":
        //         Material material = new(item);
        //         Activator.CreateInstance(Type.GetType(item.Id), material);
        //         break;
        //     case "SummonTicket":
        //         SummonTicket summonTicket = new(item);
        //         Activator.CreateInstance(Type.GetType(item.Id), summonTicket);
        //         break;
        // }

    }
}