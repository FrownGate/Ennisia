using PlayFab.EconomyModels;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolCurrencies : EditorWindow
{
    private int _gold;
    private int _crystals;
    private int _energy;

    private Label _goldLabel;
    private Label _crystalsLabel;
    private Label _energyLabel;

    private const string GOLD_ID = "0dc3228a-78a9-4d26-a3ab-f7d1e5b5c4d3"; //temp
    private const string CRYSTALS_ID = "0ec7fd19-4c26-4e0a-bd66-cf94f83ef060"; //temp

    [MenuItem("Tools/Currencies")]
    public static void ShowWindow()
    {
        if (!PlayFabManager.Instance)
        {
            Debug.LogError("Game needs to be open to use this tool.");
            return;
        }

        GetWindow<ToolCurrencies>("Currencies");
        ToolCurrencies window = GetWindow<ToolCurrencies>("Currencies");
        window.titleContent = new GUIContent("Currencies");
        window.position = new Rect(0, 0, 550, 600);
    }

    public void CreateGUI()
    {
        rootVisualElement.Clear();
        VisualElement main = new();

        CreateLabels();
        //CreateFields();
        CreateButtons();

        UpdateLabels();

        rootVisualElement.Add(main);

    }

    private void Awake()
    {
        PlayFabManager.OnGetCurrencies += UpdateCurrencies;
        PlayFabManager.OnGetEnergy += UpdateEnergy;

        PlayFabManager.Instance.GetCurrencies();
        PlayFabManager.Instance.GetEnergy();
    }

    private void OnDestroy()
    {
        PlayFabManager.OnGetCurrencies -= UpdateCurrencies;
        PlayFabManager.OnGetEnergy -= UpdateEnergy;
    }

    private void Update()
    {
        if (EditorApplication.isPlaying) return;
        Close();
    }

    private void UpdateCurrencies(List<InventoryItem> currencies)
    {
        foreach (InventoryItem item in currencies)
        {
            switch(item.Id)
            {
                case GOLD_ID:
                    _gold = (int)item.Amount;
                    break;
                case CRYSTALS_ID:
                    _crystals = (int)item.Amount;
                    break;
                default: break;
            }
        }

        UpdateLabels();
    }

    private void UpdateEnergy(int energy)
    {
        _energy = energy;
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        _goldLabel.text = "Gold: " + _gold;
        _crystalsLabel.text = "Crystals: " + _crystals;
        _energyLabel.text = "Energy: " + _energy + " / 140";
    }

    private void CreateLabels()
    {
        Label currenciesLabel = new()
        {
            text = "Currencies",
            style =
            {
                // fontStyle = FontStyle.Bold,
                fontSize = 30,
                width = 100,
                height = 30,
                marginBottom = 30,
                marginTop = 15,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                paddingBottom = 5,
                alignItems = Align.Center,
                justifyContent = Justify.Center,
            },
        };

        _goldLabel = new()
        {
            text = "Gold: ",
            style =
            {
                width = 100,
                height = 30,
                marginLeft = 20,
            }
        };

        _crystalsLabel = new()
        {
            text = "Crystals: ",
            style =
            {
                width = 100,
                height = 30,
                marginLeft = 20,
            }
        };

        _energyLabel = new()
        {
            text = "Energy: ",
            style =
            {
                width = 100,
                height = 30,
                marginLeft = 20,
            }
        };

        rootVisualElement.Add(currenciesLabel);
        rootVisualElement.Add(_goldLabel);
        rootVisualElement.Add(_crystalsLabel);
        rootVisualElement.Add(_energyLabel);
    }

    private void CreateButtons()
    {
        Button addGold = new()
        {
            text = "Add 100 000 Gold",
            style =
            {
                width = 120,
                height = 30,
                marginBottom = 5,
                marginTop = 5,
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                borderBottomLeftRadius = 5,
                borderBottomRightRadius = 5,
                borderTopColor = Color.black,
                borderTopWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                borderLeftColor = Color.black,
                borderLeftWidth = 1,
                borderRightColor = Color.black,
                borderRightWidth = 1,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                paddingBottom = 5,
                alignItems = Align.Center,
                justifyContent = Justify.Center,
            },
            transform =
            {
                position = new Vector2((position.width/8)  , (position.height/6) * 2 )
            }
        };
        addGold.clicked += AddGold;

        Button addCrystals = new()
        {
            text = "Add 2000 Crystals",
            style =
            {
                width = 120,
                height = 30,
                marginBottom = 5,
                marginTop = 5,
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                borderBottomLeftRadius = 5,
                borderBottomRightRadius = 5,
                borderTopColor = Color.black,
                borderTopWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                borderLeftColor = Color.black,
                borderLeftWidth = 1,
                borderRightColor = Color.black,
                borderRightWidth = 1,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                alignItems = Align.Center,
                justifyContent = Justify.Center,
            },
            transform =
            {
                position = new Vector2((position.width/8)*5 , ((position.height/6) * 2) - 39)
            }
        };
        addCrystals.clicked += AddCrystals;

        Button addEnergy = new()
        {
            text = "Add 40 Energy",
            style =
            {
                width = 120,
                height = 30,
                marginBottom = 5,
                marginTop = 5,
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                borderBottomLeftRadius = 5,
                borderBottomRightRadius = 5,
                borderTopColor = Color.black,
                borderTopWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                borderLeftColor = Color.black,
                borderLeftWidth = 1,
                borderRightColor = Color.black,
                borderRightWidth = 1,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                alignItems = Align.Center,
                justifyContent = Justify.Center,
            },
            transform =
            {
                position = new Vector2((position.width/8)*9 , ((position.height/6) * 2) - 78)
            }
        };
        addEnergy.clicked += AddEnergy;

        Button removeGold = new()
        {
            text = "Remove 100 000 Gold",
            style =
            {
                width = 140,
                height = 30,
                marginBottom = 5,
                marginTop = 5,
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                borderBottomLeftRadius = 5,
                borderBottomRightRadius = 5,
                borderTopColor = Color.black,
                borderTopWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                borderLeftColor = Color.black,
                borderLeftWidth = 1,
                borderRightColor = Color.black,
                borderRightWidth = 1,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                paddingBottom = 5,
                alignItems = Align.Center,
                justifyContent = Justify.Center,
            },
            transform =
            {
                position = new Vector2((position.width/8)  , ((position.height/6) * 2)   )
            }
        };
        removeGold.clicked += RemoveGold;

        Button removeCrystals = new()
        {
            text = "Remove 2000 Crystals",
            style =
            {
                width = 140,
                height = 30,
                marginBottom = 5,
                marginTop = 5,
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                borderBottomLeftRadius = 5,
                borderBottomRightRadius = 5,
                borderTopColor = Color.black,
                borderTopWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                borderLeftColor = Color.black,
                borderLeftWidth = 1,
                borderRightColor = Color.black,
                borderRightWidth = 1,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                alignItems = Align.Center,
                justifyContent = Justify.Center,
            },
            transform =
            {
                position = new Vector2((position.width/8)*5 , ((position.height/6) * 2) - 39)
            }
        };
        removeCrystals.clicked += RemoveCrystals;

        Button removeEnergy = new()
        {
            text = "Remove 40 Energy",
            style =
            {
                width = 140,
                height = 30,
                marginBottom = 5,
                marginTop = 5,
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                borderBottomLeftRadius = 5,
                borderBottomRightRadius = 5,
                borderTopColor = Color.black,
                borderTopWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                borderLeftColor = Color.black,
                borderLeftWidth = 1,
                borderRightColor = Color.black,
                borderRightWidth = 1,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                alignItems = Align.Center,
                justifyContent = Justify.Center,
            },
            transform =
            {
                position = new Vector2((position.width/8)*9 , ((position.height/6) * 2) - 78)
            }
        };
        removeEnergy.clicked += RemoveEnergy;

        rootVisualElement.Add(addGold);
        rootVisualElement.Add(addCrystals);
        rootVisualElement.Add(addEnergy);
        rootVisualElement.Add(removeGold);
        rootVisualElement.Add(removeCrystals);
        rootVisualElement.Add(removeEnergy);

    }

    private void CreateFields()
    {
        TextField addGoldField = new()
        {
            style =
            {
                width = 100,
                height = 30,
                marginBottom = 5,
                marginTop = 5,
                borderBottomColor = Color.black,
                borderBottomWidth = 1,
                borderBottomLeftRadius = 5,
                borderBottomRightRadius = 5,
                borderTopColor = Color.black,
                borderTopWidth = 1,
                borderTopLeftRadius = 5,
                borderTopRightRadius = 5,
                borderLeftColor = Color.black,
                borderLeftWidth = 1,
                borderRightColor = Color.black,
                borderRightWidth = 1,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 5,
                paddingBottom = 5,
                alignItems = Align.Center,
            }
        };
        rootVisualElement.Add(addGoldField);
    }

    private void AddGold()
    {
        PlayFabManager.Instance.AddCurrency("Gold", 100000);
    }

    private void AddCrystals()
    {
        PlayFabManager.Instance.AddCurrency("Crystals", 2000);
    }

    private void RemoveGold()
    {
        PlayFabManager.Instance.RemoveCurrency("Gold", 100000);
    }

    private void RemoveCrystals()
    {
        PlayFabManager.Instance.RemoveCurrency("Crystals", 2000);
    }

    private void AddEnergy()
    {
        PlayFabManager.Instance.AddEnergy(40);
    }

    private void RemoveEnergy()
    {
        PlayFabManager.Instance.RemoveEnergy(40);
    }
}