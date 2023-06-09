using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SummonSystem : MonoBehaviour
{
    [SerializeField] private double _legendaryChance = 0.5;
    [SerializeField] private double _epicChance = 9.5;
    //[SerializeField] private double _rareChance = 90;
    [SerializeField] private int _cost = 100;
    [SerializeField] private int _fragmentsPerDuplicate = 10;
    [SerializeField] private int _rareFragmentsMultiplier = 1;
    [SerializeField] private int _epicFragmentsMultiplier = 2;
    [SerializeField] private int _legendaryFragmentsMultiplier = 3;
    [SerializeField] public GameObject _supportImage;
    [SerializeField] public GameObject _canvasParent;

    private Dictionary<int, int> _supports;
    private List<SupportCharacterSO> _supportsPulled;
    private int _fragmentsMultiplier;
    private int _amount;
    private double _chance;

    private void Start()
    {
        //PlayFabManager.OnLoginSuccess += Testing; //testing only

        if (ScenesManager.Instance.HasParams())
        {
            _amount = int.Parse(ScenesManager.Instance.Params);
        }
        else
        {
            _amount = 1;
        }

        _chance = GetChance();

        Summon();
    }

    private void Testing()
    {
        _chance = GetChance();
        Summon();
    }

    private double GetChance()
    {
        //if (PlayFabManager.Instance.Inventory.GetItem(new SummonTicket(), Item.ItemRarity.Legendary) != null) return _legendaryChance;
        //if (PlayFabManager.Instance.Inventory.GetItem(new SummonTicket(), Item.ItemRarity.Epic) != null) return _epicChance;
        return 100;
    }

    private void OnDestroy()
    {
        //PlayFabManager.OnLoginSuccess -= Testing; //testing only
    }

    public void Summon()
    {
        //_amount = 10; //testing only
        int newFragments = 0;

        //TODO -> Use tickets instead of crystals if _chance is < 100
        if (PlayFabManager.Instance.Currencies[PlayFabManager.Currency.Crystals] < _cost * _amount)
        {
            Debug.LogError("not enough crystals");
            //TODO -> Show UI error message
            return;
        }
        else
        {
            PlayFabManager.Instance.RemoveCurrency(PlayFabManager.Currency.Crystals, _cost * _amount);
        }

        _supports = PlayFabManager.Instance.GetSupports();
        _supportsPulled = new();

        for (int i = 0; i < _amount; i++)
        {
            SupportCharacterSO pulledSupport = GetSupport();
            _supportsPulled.Add(pulledSupport);
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
        DisplayPull();

        if (newFragments == 0) return;
        PlayFabManager.Instance.AddCurrency(PlayFabManager.Currency.Fragments, newFragments);
        
    }

    private SupportCharacterSO GetSupport()
    {
        System.Random random = new();
        double rarityRoll = random.NextDouble() * _chance;
        Debug.Log($"rarity roll : {rarityRoll}");

        string pickedRarity = "Rare";
        _fragmentsMultiplier = _rareFragmentsMultiplier;

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

        SupportCharacterSO[] gachaPool = Resources.LoadAll<SupportCharacterSO>($"SO/SupportsCharacter/{pickedRarity}");
        int characterRoll = random.Next(1, gachaPool.Length);
        Debug.Log($"character roll : {characterRoll}");
        return gachaPool[characterRoll - 1];
    }
    private void DisplayPull()

    {
        int row = 0;
        int pos = 150;
                   
        for(int i = 0; i < _supportsPulled.Count; i++) 
        {
            row = i % 5 == 0 ? row +1 : row;
            pos = i % 5 == 0 ? 0 : pos + 190 ;
            Vector3 position = new Vector3(500 + pos, 300 + 190 * row ,3);

            GameObject suppportImage = Instantiate(_supportImage, position, Quaternion.identity, _canvasParent.transform);
            // supportImage.GetComponent<Image>().sprite = //_supportsPulled[i].sprite

        }
    }
}