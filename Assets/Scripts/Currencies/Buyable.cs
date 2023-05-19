using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;

namespace Currencies
{
    public class Buyable : MonoBehaviour
    {

        public void AddItemToInventory(PlayFab.ClientModels.EntityKey playerEntity, string itemAlternateId)
        {
            PlayFab.EconomyModels.EntityKey entity = new() { Id = playerEntity.Id, Type = playerEntity.Type };
            PlayFab.EconomyModels.AddInventoryItemsRequest request = new()
            {
                // CatalogVersion = "Items",
                // PlayFabId = PlayFabManager.Instance.PlayFabId,
                // ItemIds = new List<string> { "607c7159-82a0-4796-97aa-1b1d77cbe4db" }

                Entity = entity,
                Item = new PlayFab.EconomyModels.InventoryItemReference
                {
                    AlternateId = new PlayFab.EconomyModels.AlternateId
                    {
                        Type = "FriendlyId",
                        Value = itemAlternateId
                    },
                    // Id = "b9c6d241-67e4-4fd4-b459-c3cb2a462fcf",
                    // StackId = "TsStack1"
                },
                // FIXME: The amount will be a return from an int function
                Amount = 1
            };
            PlayFabEconomyAPI.AddInventoryItems(request, OnAddedItemToInventorySucess, OnAddedItemToInventoryError);
        }

        private void OnAddedItemToInventorySucess(AddInventoryItemsResponse result)
        {
            Debug.Log("Successfully granted item: ");
        }
        private void OnAddedItemToInventoryError(PlayFabError error)
        {
            Debug.LogError("Error while adding item: " + error.ErrorMessage);
        }

        public void PurchaseItem(PlayFab.ClientModels.EntityKey playerEntity, string itemAlternateId, int itemAmount, string currencyId, int currencyAmount)
        {
            PlayFab.EconomyModels.EntityKey entity = new() { Id = playerEntity.Id, Type = playerEntity.Type };
            PlayFab.EconomyModels.PurchaseInventoryItemsRequest request = new()
            {
                // CatalogVersion = "Items",
                // PlayFabId = PlayFabManager.Instance.PlayFabId,
                // ItemIds = new List<string> { "607c7159-82a0-4796-97aa-1b1d77cbe4db" }

                Entity = entity,
                Item = new PlayFab.EconomyModels.InventoryItemReference
                {
                    AlternateId = new PlayFab.EconomyModels.AlternateId
                    {
                        Type = "FriendlyId",
                        Value = itemAlternateId
                    },
                    // Id = "b9c6d241-67e4-4fd4-b459-c3cb2a462fcf",
                    // StackId = "TsStack1"
                },
                Amount = itemAmount,
                PriceAmounts = new List<PlayFab.EconomyModels.PurchasePriceAmount>
            {
                new PlayFab.EconomyModels.PurchasePriceAmount
                {
                    //TODO: check the currency and add the right ItemId

                    ItemId = "0dc3228a-78a9-4d26-a3ab-f7d1e5b5c4d3", // Gold
                    // ItemId = "0ec7fd19-4c26-4e0a-bd66-cf94f83ef060", // Crystals
                    Amount = currencyAmount
                }
            }

            };
            PlayFabEconomyAPI.PurchaseInventoryItems(request, OnPurchaseItemSucess, OnPurchaseItemError);
        }

        private void OnPurchaseItemSucess(PurchaseInventoryItemsResponse result)
        {
            Debug.Log("Successfully purchased item: ");
        }

        private void OnPurchaseItemError(PlayFabError error)
        {
            Debug.LogError("Error while purchasing item: " + error.ErrorMessage);
        }
    }
}