using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;


using Currencies;

public class ToolCurrencies : EditorWindow
{
    public static int goldAmount;
    public static int crystalsAmount;
    public static int energyAmount;

    private Label goldLabel;
    private Label crystalsLabel;
    private Label energyLabel;

    [MenuItem("Currencies/Gestion")]
    public static void ShowWindow()
    {
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
        // CreateFields();
        CreateButtons();

        goldLabel.text = "Gold: " + goldAmount;
        crystalsLabel.text = "Crystals: " + crystalsAmount;
        energyLabel.text = "Energy: " + energyAmount + " / 140";

        rootVisualElement.Add(main);

    }

    private void Awake()
    {
        PlayFabManager.Instance.GetCurrency();
        PlayFabManager.Instance.GetEnergy();

    }

    private void Update()
    {
        goldLabel.text = "Gold: " + goldAmount;
        crystalsLabel.text = "Crystals: " + crystalsAmount;
        energyLabel.text = "Energy: " + energyAmount + " / 140";
        if (EditorApplication.isPlaying && !EditorApplication.isPaused) { Repaint(); }

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


        goldLabel = new()
        {
            text = "Gold: ",
            style =
            {
                width = 100,
                height = 30,
                marginLeft = 20,
            }
        };


        crystalsLabel = new()
        {
            text = "Crystals: ",
            style =
            {
                width = 100,
                height = 30,
                marginLeft = 20,
            }
        };

        energyLabel = new()
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
        rootVisualElement.Add(goldLabel);
        rootVisualElement.Add(crystalsLabel);
        rootVisualElement.Add(energyLabel);
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
        PlayFabManager.Instance.GetCurrency();
        PlayFabManager.Instance.AddCurrency("Gold", 100000);
    }
    private void AddCrystals()
    {
        PlayFabManager.Instance.GetCurrency();
        PlayFabManager.Instance.AddCurrency("Crystals", 2000);
    }

    private void RemoveGold()
    {
        PlayFabManager.Instance.GetCurrency();
        PlayFabManager.Instance.RemoveCurrency("Gold", 100000);
    }

    private void RemoveCrystals()
    {
        PlayFabManager.Instance.GetCurrency();
        PlayFabManager.Instance.RemoveCurrency("Crystals", 2000);
    }

    private void AddEnergy()
    {
        PlayFabManager.Instance.GetEnergy();
        PlayFabManager.Instance.AddEnergy(40);
    }

    private void RemoveEnergy()
    {
        PlayFabManager.Instance.GetEnergy();
        PlayFabManager.Instance.RemoveEnergy(40);
    }

}