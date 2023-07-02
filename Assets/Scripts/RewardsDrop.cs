using UnityEngine;


public class RewardsDrop : MonoBehaviour
{
    public static RewardsDrop Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //random gear drop
    public void DropGear(Item.ItemRarity rarity)
    {
        Item.GearType type = Item.GearType.Helmet;
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: type = Item.GearType.Chest; break;
            case 1: type = Item.GearType.Boots; break;
            case 2: type = Item.GearType.Ring; break;
            case 3: type = Item.GearType.Necklace; break;
            case 4: type = Item.GearType.Earrings; break;
        }
        PlayFabManager.Instance.AddInventoryItem(new Gear(type, rarity));
    }
    public void DropGear(Item.GearType type, Item.ItemRarity rarity)
    {
        PlayFabManager.Instance.AddInventoryItem(new Gear(type, rarity));
    }
    public void DropCurrency(PlayFabManager.GameCurrency currency, int amount)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
    }
    public void DropCurrency(PlayFabManager.GameCurrency currency, int amount, PlayFabManager.GameCurrency currency2, int amount2)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
    }
    public void DropCurrency(PlayFabManager.GameCurrency currency, int amount, PlayFabManager.GameCurrency currency2, int amount2, PlayFabManager.GameCurrency currency3, int amount3)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
        PlayFabManager.Instance.AddCurrency(currency3, amount3);
    }
    public void DropCurrency(PlayFabManager.GameCurrency currency, int amount, PlayFabManager.GameCurrency currency2, int amount2, PlayFabManager.GameCurrency currency3, int amount3, PlayFabManager.GameCurrency currency4, int amount4)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
        PlayFabManager.Instance.AddCurrency(currency3, amount3);
        PlayFabManager.Instance.AddCurrency(currency4, amount4);
    }
    public void DropCurrency(PlayFabManager.GameCurrency currency, int amount, PlayFabManager.GameCurrency currency2, int amount2, PlayFabManager.GameCurrency currency3, int amount3, PlayFabManager.GameCurrency currency4, int amount4, PlayFabManager.GameCurrency currency5, int amount5)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
        PlayFabManager.Instance.AddCurrency(currency3, amount3);
        PlayFabManager.Instance.AddCurrency(currency4, amount4);
        PlayFabManager.Instance.AddCurrency(currency5, amount5);
    }
    public void DropCurrency(PlayFabManager.GameCurrency currency, int amount, PlayFabManager.GameCurrency currency2, int amount2, PlayFabManager.GameCurrency currency3, int amount3, PlayFabManager.GameCurrency currency4, int amount4, PlayFabManager.GameCurrency currency5, int amount5, PlayFabManager.GameCurrency currency6, int amount6)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
        PlayFabManager.Instance.AddCurrency(currency3, amount3);
        PlayFabManager.Instance.AddCurrency(currency4, amount4);
        PlayFabManager.Instance.AddCurrency(currency5, amount5);
        PlayFabManager.Instance.AddCurrency(currency6, amount6);
    }
    public void DropEnergy(int amount)
    {
        PlayFabManager.Instance.AddEnergy(amount);
    }

}
