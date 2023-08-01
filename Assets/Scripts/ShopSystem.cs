using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

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
        foreach (var shop in _shops)
        {
            GameObject shopBtn = Instantiate(_shopBtnPrefab, transform);
            shopBtn.transform.SetParent(_ShopBtnPanel.transform);
            var shopBtnInfo = shopBtn.GetComponent<ShowShopInfo>();
            shopBtnInfo.ShopName = shop.Value.AlternateIds[0].Value;
        }
        //First Shop's items
        _currentShop = "32ae8b20-5684-4937-8150-3b17958bdbbe";
        InitShopItems(_shops[_currentShop].ItemReferences);
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
            //SetImageFromURL(item.Images[0].Url, itemInfo); //TODO: Fix the bug with the images
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
                return;
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
    
    public void SetImageFromURL(string url, ShowShopBtnInfo item)
    {
        StartCoroutine(DownloadImage(url, item));
    }

    private IEnumerator DownloadImage(string url, ShowShopBtnInfo item)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            item._itemImage.sprite = Texture2DToSprite(texture);
        }
    }
    
    private Sprite Texture2DToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}