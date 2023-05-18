using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;

using ConsoleGame.Currencies;
public class Buyable : MonoBehaviour
{
    public int price;
    public string currency;
    public string name;


    // public void Buy()
    // {

    // rewrite and use the Economy V2 api




    //     SubtractUserVirtualCurrencyRequest request = new()
    //     {
    //         // check if it's the right currency

    //         // if (currency == "Gold")
    //         // {
    //         //     VirtualCurrency = "GO";
    //         //     Amount = price;
    //         // }
    //         // else if (currency == "Crystals")
    //         // {
    //         //     VirtualCurrency = "CY";
    //         //     Amount = price;
    //         // }
    //         // else
    //         // {
    //         //     Debug.Log("Wrong currency");
    //         // }
    //         VirtualCurrency = currency,
    //         Amount = price
    //     };
    //     PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractUserVirtualCurrency, OnError);
    // }

    // public void OnSubtractUserVirtualCurrency(ModifyUserVirtualCurrencyResult result)
    // {
    //     Debug.Log("You bought " + name + " for " + price + " " + currency);
    // }

    // public void OnError(PlayFabError error)
    // {
    //     Debug.Log("Error while buying " + name + " for " + price + " " + currency);
    //     Debug.Log(error.GenerateErrorReport());
    // }


//TODO: keep this for reference
    // public void PurchaseCurrency(int currencyAmount)
    // {
    //     // Effectuer l'achat en utilisant le SKU de l'article et le montant de Currency spécifiés
    //     var request = new PurchaseItemRequest
    //     {
    //         ItemId = "607c7159-82a0-4796-97aa-1b1d77cbe4db",
    //         Price = currencyAmount,
    //         VirtualCurrency = "GO"
    //     };
    //     PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnPurchaseItemError);
    // }

    // // Callback appelé lorsque l'achat réussit
    // private void OnPurchaseItemSuccess(PurchaseItemResult result)
    // {
    //     // Mettre à jour l'inventaire du joueur ou déclencher des événements dans votre jeu
    //     Debug.Log("L'achat a été effectué avec succès!");
    // }

    // // Callback appelé lorsque l'achat échoue
    // private void OnPurchaseItemError(PlayFabError error)
    // {
    //     Debug.LogError("Error while buying: " + error.ErrorMessage);
    // }
    // }

    public void AddInventoryItems()
    {
        var request = new AddInventoryItemsRequest 
        {
            // CatalogVersion = "Items",
            // PlayFabId = PlayFabManager.Instance.PlayFabId,
            // ItemIds = new List<string> { "607c7159-82a0-4796-97aa-1b1d77cbe4db" }
        };
        PlayFabEconomyAPI.AddInventoryItems(request, OnAddedSucess, OnError);
    }

    private void OnAddedSucess(AddInventoryItemsResponse result)
    {
        Debug.Log("Successfully granted item:" + result.);
    }
    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error while listing currencies: " + error.ErrorMessage);
    }
}