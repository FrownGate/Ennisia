using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace ConsoleGame.Currencies
{
    public class Currencies
    {
        //     // Currency registry, listing all currencies by their registry ID.
        //     private static readonly Dictionary<string, Currencies> _currenciesByRegistryID;
        //     public static Dictionary<string, Currencies> CurrencyList { get { return _currenciesByRegistryID; } }

        //     // Currency type stats.
        //     public string Name { get; private set; }
        //     public int Value { get; private set; }

        //     // Currency registration.
        //     static Currencies()
        //     {
        //         _currenciesByRegistryID = new Dictionary<string, Currencies>();

        //         //  ---- Create Currencies ----
        //         //                                  Name | Value
        //         Currencies gold = new("Gold", 0);
        //         Currencies crystals = new("Crystals", 0);
        //     }

        //     // Constructor
        //     public Currencies(string name, int value)
        //     {
        //         Name = name;
        //         Value = value;
        //         _currenciesByRegistryID.Add(name, this);
        //     }

        //     // Add currency
        //     public void Add(int amount)
        //     {
        //         Value += amount;
        //     }

        //     // Consume currency
        //     public void Consume(int amount)
        //     {
        //         // check for negative values
        //         if (Value - amount < 0)
        //         {
        //             throw new Exception("Insufficient amount of" + Name + ".");
        //         }
        //     }

        public void GetCurrencyBalance()
        {
            var request = new GetPlayerCombinedInfoRequest()
            {
                // PlayFabId = PlayFabManager.Instance.PlayFabId,
                // VirtualCurrency = "GO"

                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
                {
                    GetUserInventory = true,
                    GetUserVirtualCurrency = true,
                }

            };
            PlayFabClientAPI.GetPlayerCombinedInfo(request, OnGetCurrencyBalance, OnError);
        }

        private void OnGetCurrencyBalance(GetPlayerCombinedInfoResult result)
        {
            // Obtenir le solde de Currency du joueur à partir de la réponse de l'API
            int currencyBalance = result.InfoResultPayload.UserVirtualCurrency["GO"];

            // Utiliser le solde de Currency dans votre jeu
            Debug.Log("Le joueur a un solde de " + currencyBalance + " GO");
        }

        private void OnError(PlayFabError error)
        {
            Debug.LogError("Erreur lors de l'obtention d'informations du joueur: " + error.ErrorMessage);
        }
    }
}
