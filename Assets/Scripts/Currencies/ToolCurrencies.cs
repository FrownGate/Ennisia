using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;


using Currencies;

public class ToolCurrencies : EditorWindow
{
    // Text of gold amount
    public static int goldAmount;
    // Text of crystals amount
    public static int crystalsAmount;

    private Label goldLabel;
    private Label crystalsLabel;

    [MenuItem("Currencies/Gestion")]
    public static void ShowWindow()
    {
        GetWindow<ToolCurrencies>("Currencies");
        ToolCurrencies window = GetWindow<ToolCurrencies>("Currencies");
        window.titleContent = new GUIContent("Currencies");
        window.minSize = new Vector2(500, 500);
        window.maxSize = new Vector2(500, 500);
        window.position = new Rect(0, 0, 500, 600);
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

        rootVisualElement.Add(main);

    }

    private void Awake()
    {
        PlayFabManager.Instance.GetCurrency();

    }

    private void Update()
    { 
        goldLabel.text = "Gold: " + goldAmount;
        crystalsLabel.text = "Crystals: " + crystalsAmount;
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



        rootVisualElement.Add(currenciesLabel);
        rootVisualElement.Add(goldLabel);
        rootVisualElement.Add(crystalsLabel);
    }


    private void CreateButtons()
    {


        // Create button to show currencies
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
                position = new Vector2((position.width/6)  , (position.height/6) * 2 )
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
                position = new Vector2((position.width/6*5) , ((position.height/6) * 2) - 39)
            }
        };
        addCrystals.clicked += AddCrystals;


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
                position = new Vector2((position.width/6)  , ((position.height/6) * 2)   )
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
                position = new Vector2((position.width/6*5) , ((position.height/6) * 2) - 39)
            }
        };
        removeCrystals.clicked += RemoveCrystals;

        rootVisualElement.Add(addGold);
        rootVisualElement.Add(addCrystals);
        rootVisualElement.Add(removeGold);
        rootVisualElement.Add(removeCrystals);

    }

    private void CreateFields()
    {
        // Create field to add currencies
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

}