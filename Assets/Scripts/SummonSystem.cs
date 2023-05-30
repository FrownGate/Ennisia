using UnityEngine;
using System.Collections.Generic;
using System;

public class SummonSystem : MonoBehaviour
{
    [SerializeField] private double _legendaryChance = 0.5;
    [SerializeField] private double _epicChance = 9.5;
    [SerializeField] private double _rareChance = 90;
    [SerializeField] private int _cost = 100;
    [SerializeField] private int _fragmentsPerDuplicate = 10;
    [SerializeField] private int _rareFragmentsMultiplier = 1;
    [SerializeField] private int _epicFragmentsMultiplier = 2;
    [SerializeField] private int _legendaryFragmentsMultiplier = 3;

    private Dictionary<int, int> _supports;
    private int _fragmentsMultiplier;
    private int _amount;

    private void Start()
    {
        //PlayFabManager.OnLoginSuccess += Summon; //testing only

        if (ScenesManager.Instance.HasParams())
        {
            _amount = int.Parse(ScenesManager.Instance.Params);
        }
        else
        {
            _amount = 1;
        }

        Summon();
    }

    private void OnDestroy()
    {
        //PlayFabManager.OnLoginSuccess -= Summon; //testing only
    }

    public void Summon()
    {
        //_amount = 10; //testing only
        int newFragments = 0;

        if (PlayFabManager.Instance.Currencies["Crystals"] < _cost * _amount)
        {
            Debug.LogError("not enough crystals");
            //TODO -> Show UI error message
            return;
        }
        else
        {
            PlayFabManager.Instance.RemoveCurrency("Crystals", _cost * _amount);
        }

        _supports = PlayFabManager.Instance.GetSupports();

        for (int i = 0; i < _amount; i++)
        {
            SupportsCharactersSO pulledSupport = GetSupport();
            Debug.Log($"{pulledSupport.Name} has been pulled !");

            if (_supports.ContainsKey(pulledSupport.Id))
            {
                if (_supports[pulledSupport.Id] < 5)
                {
                    _supports[pulledSupport.Id]++;
                }
                else
                {
                    newFragments += _fragmentsPerDuplicate * _fragmentsMultiplier;
                }
            }
            else
            {
                _supports[pulledSupport.Id] = 1;
            }
        }

        PlayFabManager.Instance.AddSupports(_supports);

        if (newFragments == 0) return;
        PlayFabManager.Instance.AddCurrency("Fragments", newFragments);
    }

    private SupportsCharactersSO GetSupport()
    {
        System.Random random = new();
        double rarityRoll = random.NextDouble() * 100;
        Debug.Log($"rarity roll : {rarityRoll}");
        //TODO -> add guaranteed rarity modifier

        string pickedRarity = "";

        if (rarityRoll <= _legendaryChance)
        {
            pickedRarity = "Legendary";
            _fragmentsMultiplier = _legendaryFragmentsMultiplier;
        }
        else if (rarityRoll <= _epicChance)
        {
            pickedRarity = "Epic";
            _fragmentsMultiplier = _epicFragmentsMultiplier;
        }
        else if (rarityRoll <= _rareChance)
        {
            pickedRarity = "Rare";
            _fragmentsMultiplier = _rareFragmentsMultiplier;
        }

        SupportsCharactersSO[] gachaPool = Resources.LoadAll<SupportsCharactersSO>($"SO/SupportsCharacter/{pickedRarity}");
        int characterRoll = random.Next(gachaPool.Length);
        Debug.Log($"character roll : {characterRoll}");
        return gachaPool[characterRoll - 1];
    }
}