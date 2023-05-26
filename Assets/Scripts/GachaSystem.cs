using UnityEngine;
using System.Collections.Generic;

public class GachaSystem : MonoBehaviour
{
    [SerializeField] private double _legendaryChance = 0.5;
    [SerializeField] private double _epicChance = 9.5;
    [SerializeField] private double _rareChance = 90;
    [SerializeField] private int _cost = 100;
    [SerializeField] private int _fragmentsPerDuplicate = 10;

    private Dictionary<int, int> _supports;
    private int _fragmentsMultiplier;

    private void Start()
    {
        //PlayFabManager.OnLoginSuccess += Summon; //testing only
    }

    private void OnDestroy()
    {
        //PlayFabManager.OnLoginSuccess -= Summon; //testing only
    }

    //public void Summon() //testing only
    public void Summon(int amount)
    {
        //int amount = 10; //testing only
        int newFragments = 0;

        if (PlayFabManager.Instance.Currencies["Crystals"] < _cost * amount)
        {
            Debug.LogError("not enough crystals");
            //TODO -> Show UI error message
            return;
        }
        else
        {
            PlayFabManager.Instance.RemoveCurrency("Crystals", _cost * amount);
        }

        _supports = PlayFabManager.Instance.GetSupports();

        for (int i = 0; i < amount; i++)
        {
            SupportsCharactersSO pulledSupport = GetSupport();
            Debug.Log($"{pulledSupport.suppportName} has been pulled !");

            if (_supports.ContainsKey(pulledSupport.id))
            {
                if (_supports[pulledSupport.id] < 5)
                {
                    _supports[pulledSupport.id]++;
                }
                else
                {
                    newFragments += _fragmentsPerDuplicate * _fragmentsMultiplier;
                }
            }
            else
            {
                _supports[pulledSupport.id] = 1;
            }
        }

        PlayFabManager.Instance.AddSupports(_supports);

        if (newFragments == 0) return;
        PlayFabManager.Instance.AddCurrency("Fragments", newFragments);
    }

    private SupportsCharactersSO GetSupport()
    {
        System.Random random = new();
        double roll = random.NextDouble() * 100;
        Debug.Log($"gacha rolled : {roll}");
        //TODO -> add guaranteed rarity modifier

        string pickedRarity = "";

        if (roll <= _legendaryChance)
        {
            pickedRarity = "Legendary";
            _fragmentsMultiplier = 3;
        }
        else if (roll <= _epicChance)
        {
            pickedRarity = "Epic";
            _fragmentsMultiplier = 2;
        }
        else if (roll <= _rareChance)
        {
            pickedRarity = "Rare";
            _fragmentsMultiplier = 1;
        }

        SupportsCharactersSO[] gachaPool = Resources.LoadAll<SupportsCharactersSO>($"SupportsCharacter/{pickedRarity}");
        return gachaPool[random.Next(gachaPool.Length)];
    }
}