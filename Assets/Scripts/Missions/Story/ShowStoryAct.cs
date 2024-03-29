using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class ShowStoryAct : MonoBehaviour
{
    public int TargetActId;
    public GameObject ButtonPrefab; // The prefab for the UI button
    [Scene] public string scene;
    public static event Action<string> Onclick;

    public void Start()
    {
        // Retrieve all ScriptableObjects of type ChapterSO
        ChapterSO[] scriptableObjects = Resources.LoadAll<ChapterSO>($"SO/Chapters/Act {TargetActId}/");
        TextMeshProUGUI actText = GetComponentInChildren<TextMeshProUGUI>();
        actText.text = $"ACT {TargetActId}";

        if (scriptableObjects.Length > 0)
        {
            // Iterate through the ScriptableObjects and filter by the target act ID
            foreach (ChapterSO obj in scriptableObjects)
            {
                if (obj.ActId == TargetActId) CreateUIButton(obj);
            }

            return;
        }

        CreateCommingSoon();
    }

    private void CreateCommingSoon()
    {
        GameObject buttonObj = Instantiate(ButtonPrefab, transform);
        buttonObj.name = "upcomming";
        Button buttonComponent = buttonObj.GetComponent<Button>();
        buttonComponent.interactable = false;
        TextMeshProUGUI buttonText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Coming Soon";
    }

    private void CreateUIButton(ChapterSO scriptableObject)
    {
        GameObject buttonObj = Instantiate(ButtonPrefab, transform);
        buttonObj.name = scriptableObject.name;

        Button buttonComponent = buttonObj.GetComponent<Button>();

        TextMeshProUGUI buttonText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = scriptableObject.name.Replace("-", " ");

        // Add an onClick event listener to the button
        buttonComponent.onClick.AddListener(() => ButtonClicked(scriptableObject));
    }

    private void ButtonClicked(ChapterSO scriptableObject)
    {
        Onclick?.Invoke(scene);
        MissionManager.Instance.SetChapter(scriptableObject);
        ScenesManager.Instance.SetScene(scene);
    }
}