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
    public void DropGear(Rarity rarity)
    {
        GearType type = GearType.Helmet;
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: type = GearType.Chest; break;
            case 1: type = GearType.Boots; break;
            case 2: type = GearType.Ring; break;
            case 3: type = GearType.Necklace; break;
            case 4: type = GearType.Earrings; break;
        }
        PlayFabManager.Instance.AddInventoryItem(new Gear(type, rarity));
    }
    public void DropGear(GearType type, Rarity rarity)
    {
        PlayFabManager.Instance.AddInventoryItem(new Gear(type, rarity));
    }
    public void DropCurrency(Currency currency, int amount)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
    }
    public void DropCurrency(Currency currency, int amount, Currency currency2, int amount2)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
    }
    public void DropCurrency(Currency currency, int amount, Currency currency2, int amount2, Currency currency3, int amount3)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
        PlayFabManager.Instance.AddCurrency(currency3, amount3);
    }
    public void DropCurrency(Currency currency, int amount, Currency currency2, int amount2, Currency currency3, int amount3, Currency currency4, int amount4)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
        PlayFabManager.Instance.AddCurrency(currency3, amount3);
        PlayFabManager.Instance.AddCurrency(currency4, amount4);
    }
    public void DropCurrency(Currency currency, int amount, Currency currency2, int amount2, Currency currency3, int amount3, Currency currency4, int amount4, Currency currency5, int amount5)
    {
        PlayFabManager.Instance.AddCurrency(currency, amount);
        PlayFabManager.Instance.AddCurrency(currency2, amount2);
        PlayFabManager.Instance.AddCurrency(currency3, amount3);
        PlayFabManager.Instance.AddCurrency(currency4, amount4);
        PlayFabManager.Instance.AddCurrency(currency5, amount5);
    }
    public void DropCurrency(Currency currency, int amount, Currency currency2, int amount2, Currency currency3, int amount3, Currency currency4, int amount4, Currency currency5, int amount5, Currency currency6, int amount6)
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
