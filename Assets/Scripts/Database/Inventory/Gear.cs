using PlayFab.EconomyModels;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Gear : Item
{
    public static event Action LevelUp;
    public static event Action<GearType?> MaxLevel;
    public static event Action<GearType?> ObtainGear;
    public string Icon; //TODO -> create function to return icon path with name
    public float Value;
    public float BaseValue; //used for upgrade
    public Dictionary<Attribute, float> BaseSubStats; //used for upgrade
    public Dictionary<Attribute, float> SubStats;
    public JsonSubstatsDictionary[] JsonSubstats;
    public string Description;
    public int Level;
    public float StatUpgrade;
    public float RatioUpgrade = 0.04f;
    public GearSO WeaponSO;
    public GearSet GearSet;

    [Serializable]
    public class JsonSubstatsDictionary
    {
        public string Key;
        public float Value;
    }

    public Gear(GearType type, Rarity rarity, GearSet gearSet, GearSO weapon = null)
    {
        Debug.Log("gear created");

        WeaponSO = weapon;
        WeaponType = weapon ? weapon.WeaponType : null;
        Id = SetId();
        Stack = Id.ToString();
        Type = type;
        Rarity = rarity;
        GearSet = gearSet;
        Description = "";
        Category = SetCategory();
        Attribute = SetAttribute();
        BaseValue = SetValue();
        Value = BaseValue;
        BaseSubStats = SetSubstats();
        SubStats = BaseSubStats;
        Level = 1;
        Amount = 1;

        AddToInventory();
        ObtainGear?.Invoke(type);
    }

    public Gear(GearSO weapon, Rarity rarity) : this(weapon.Type, rarity, null, weapon) { }
    public Gear(GearSO gear) : this(gear.Type, gear.Rarity, null, gear.Type == GearType.Weapon ? gear : null) { }

    public Gear(InventoryItem item)
    {
        Gear gear = JsonUtility.FromJson<Gear>(item.DisplayProperties.ToString());
        gear.Deserialize();

        Id = int.Parse(item.StackId);
        Stack = item.StackId;
        Type = gear.Type;
        Rarity = gear.Rarity;
        Description = gear.Description;
        Category = gear.Category;
        Attribute = gear.Attribute;
        BaseValue = gear.BaseValue;
        Value = BaseValue;
        BaseSubStats = gear.SubStats;
        SubStats = BaseSubStats;
        Level = gear.Level;
        Name = gear.Name;

        if (Category == ItemCategory.Weapon)
        {
            string weaponName = Name.Split($"[{Rarity}] ")[1];
            WeaponSO = Resources.Load<GearSO>($"SO/Weapons/{weaponName}");
        }

        AddToInventory();
        //ObtainGear?.Invoke(Type); ????
    }

    public Gear(CatalogItem item)
    {
        Gear gear = JsonUtility.FromJson<Gear>(item.DisplayProperties.ToString());
        gear.Deserialize();

        Id = SetId();
        Stack = item.Id;
        Type = gear.Type;
        Rarity = gear.Rarity;
        Description = gear.Description;
        Category = gear.Category;
        Attribute = gear.Attribute;
        BaseValue = gear.BaseValue;
        Value = BaseValue;
        BaseSubStats = gear.SubStats;
        SubStats = BaseSubStats;
        Level = gear.Level;
        Name = gear.Name;

        if (Category == ItemCategory.Weapon)
        {
            string weaponName = Name.Split($"[{Rarity}] ")[1];
            WeaponSO = Resources.Load<GearSO>($"SO/Weapons/{weaponName}");
        }

        AddToInventory();
        
    }

    public override void Serialize()
    {
        if (Category != ItemCategory.Weapon && Rarity != global::Rarity.Common)
        {
            JsonSubstats = new JsonSubstatsDictionary[(int)Rarity];
            int i = 0;

            foreach (KeyValuePair<Attribute, float> substat in SubStats)
            {
                JsonSubstats[i] = new JsonSubstatsDictionary
                {
                    Key = substat.Key.ToString(),
                    Value = substat.Value
                };

                i++;
            }
        }

        WeaponSO = null;
        base.Serialize();
    }

    public override void Deserialize()
    {
        base.Deserialize();

        if (Category == ItemCategory.Weapon || Rarity == global::Rarity.Common) return;
        SubStats = new();

        for (int i = 0; i < JsonSubstats.Length; i++)
        {
            SubStats[System.Enum.Parse<Attribute>(JsonSubstats[i].Key)] = JsonSubstats[i].Value;
            Debug.Log(JsonSubstats[i].Key);
        }
    }

    private int SetId()
    {
        //TODO -> Use last item in inventory ID + 1
        if (PlayFabManager.Instance.Data.Inventory.Items.ContainsKey("Gear"))
        {
            return PlayFabManager.Instance.Data.Inventory.Items["Gear"].Count + 1;
        }
        else
        {
            return 1;
        }
    }

    private ItemCategory SetCategory()
    {
        if ((int)Type < 3) return ItemCategory.Armor;
        else if ((int)Type < 6) return ItemCategory.Accessory;
        else return ItemCategory.Weapon;
    }

    private Attribute SetAttribute()
    {
        if (Category == ItemCategory.Weapon) return WeaponSO.Attribute;

        List<Attribute> possiblesAttributes = Resources.Load<EquipmentAttributesSO>($"SO/EquipmentStats/Attributes/{Type}").Attributes;
        return possiblesAttributes[UnityEngine.Random.Range(0, possiblesAttributes.Count - 1)];
    }

    private float SetValue()
    {
        if (Category == ItemCategory.Weapon) return WeaponSO.StatValue;

        StatMinMaxValuesSO possibleValues = Resources.Load<StatMinMaxValuesSO>($"SO/EquipmentStats/Values/{Rarity}_{Attribute}");
        return UnityEngine.Random.Range(possibleValues.MinValue, possibleValues.MaxValue); //TODO -> use random float
    }

    private Dictionary<Attribute, float> SetSubstats()
    {
        Dictionary<Attribute, float> substats = new();
        if (Category == ItemCategory.Weapon) return null;

        for (int i = 0; i < (int)Rarity; i++)
        {
            Attribute? stat;
            StatMinMaxValuesSO possibleValues;

            do
            {
                stat = (Attribute)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(Attribute)).Length);
                possibleValues = Resources.Load<StatMinMaxValuesSO>($"SO/EquipmentStats/Values/{Rarity}_{stat}");
            } while (possibleValues == null);

            substats[(Attribute)stat] = UnityEngine.Random.Range(possibleValues.MinValue, possibleValues.MaxValue); //TODO -> use random float;
        }

        return substats;
    }

    protected override void SetName()
    {
        Name = Category == ItemCategory.Weapon ? $"[{Rarity}] {WeaponSO.Name}" : $"[{Rarity}] {Type}";
    }

    public override void Upgrade()
    {
        //TODO -> add upgrade chances (50%)
        //TODO -> check and use materials
        //TODO -> substats upgrade formula
        if (Level >= 50) return;
        Debug.Log($"Upgrading {Name}...");
        LevelUp?.Invoke();
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand > (100 - Level)) return;
        Level++;
        Value += BaseValue * RatioUpgrade * Level;
        if (Level % 5 == 0)
        {
            foreach (var substat in BaseSubStats)
            {
                SubStats[substat.Key] = substat.Value * RatioUpgrade * Level; ;
            }
        }
        if(Level==50) MaxLevel?.Invoke(Type);
        PlayFabManager.Instance.UpdateItem(this);
    }

    public void Upgrade(int _level) //Testing only
    {
        if (_level >= 50) return;
        Debug.Log($"Upgrading {Name}...");


        Value += BaseValue * RatioUpgrade * _level;

        PlayFabManager.Instance.UpdateItem(this);
    }
}