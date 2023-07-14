using System.Collections.Generic;
using UnityEngine;

public class SummonModule : Module
{
    [SerializeField] private double _legendaryChance = 0.75;
    [SerializeField] private double _epicChance = 10.25;

    public readonly int SummonCost = 100;
    public readonly int FragmentsGain = 10;
    public Dictionary<Rarity, double> Chances;

    private void Awake()
    {
        Chances = new()
        {
            [Rarity.Legendary] = _legendaryChance,
            [Rarity.Epic] = _epicChance
        };
    }

    public Dictionary<int, int> GetSupports()
    {
        Dictionary<int, int> supports = new();

        foreach (SupportData support in _manager.Inventory.Supports)
        {
            supports[support.Id] = support.Lvl;
        }

        return supports;
    }

    public int HasSupport(int id)
    {
        for (int i = 0; i < _manager.Inventory.Supports.Count; i++)
        {
            if (_manager.Inventory.Supports[i].Id == id) return i;
        }

        return 0;
    }

    public void AddSupports(Dictionary<int, int> pulledSupports)
    {
        List<SupportData> supports = new();

        foreach (KeyValuePair<int, int> support in pulledSupports)
        {
            supports.Add(new()
            {
                Id = support.Key,
                Lvl = support.Value
            });
        }

        _manager.Inventory.Supports = supports;
        _manager.UpdateData();
    }
}
