using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.EconomyModels;

namespace Currencies
{
    public class Buyable : MonoBehaviour
    {
        private EntityKey _entity;

        private void Awake()
        {
            _entity = new() { Id = PlayFabManager.Instance.Entity.Id, Type = PlayFabManager.Instance.Entity.Type };
        }
        public void AddItemToInventory(string itemAlternateId)
        {
            PlayFabEconomyAPI.AddInventoryItems(new()
            {
                // CatalogVersion = "Items",
                // PlayFabId = PlayFabManager.Instance.PlayFabId,
                // ItemIds = new List<string> { "607c7159-82a0-4796-97aa-1b1d77cbe4db" }

                Entity = _entity,
                Item = new InventoryItemReference
                {
                    AlternateId = new AlternateId
                    {
                        Type = "FriendlyId",
                        Value = itemAlternateId
                    },
                    // Id = "b9c6d241-67e4-4fd4-b459-c3cb2a462fcf",
                    // StackId = "TsStack1"
                },
                // FIXME: The amount will be a return from an int function
                Amount = 1
            }, OnAddedItemToInventorySucess, PlayFabManager.Instance.OnRequestError);
        }

        private void OnAddedItemToInventorySucess(AddInventoryItemsResponse result)
        {
            Debug.Log("Successfully granted item: ");
        }

        public void PurchaseItem(string itemAlternateId, int itemAmount, string currencyId, int currencyAmount)
        {
            PlayFabEconomyAPI.PurchaseInventoryItems(new()
            {
                // CatalogVersion = "Items",
                // PlayFabId = PlayFabManager.Instance.PlayFabId,
                // ItemIds = new List<string> { "607c7159-82a0-4796-97aa-1b1d77cbe4db" }

                Entity = _entity,
                Item = new()
                {
                    AlternateId = new AlternateId
                    {
                        Type = "FriendlyId",
                        Value = itemAlternateId
                    },
                    // Id = "b9c6d241-67e4-4fd4-b459-c3cb2a462fcf",
                    // StackId = "TsStack1"
                },
                Amount = itemAmount,
                PriceAmounts = new List<PurchasePriceAmount>
            {
                new PurchasePriceAmount
                {
                    //TODO: check the currency and add the right ItemId

                    ItemId = "0dc3228a-78a9-4d26-a3ab-f7d1e5b5c4d3", // Gold
                    // ItemId = "0ec7fd19-4c26-4e0a-bd66-cf94f83ef060", // Crystals
                    Amount = currencyAmount
                }
            }

            }, OnPurchaseItemSucess, PlayFabManager.Instance.OnRequestError);
        }

        private void OnPurchaseItemSucess(PurchaseInventoryItemsResponse result)
        {
            Debug.Log("Successfully purchased item: ");
        }
    }
}