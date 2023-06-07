using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Act : MonoBehaviour
{
    public int TargetActId;
    public GameObject ButtonPrefab; // The prefab for the UI button

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
                if (obj.ActId == TargetActId)
                {
                    CreateUIButton(obj);
                }
            }
        }
        else
        {
            CreateCommingSoon();
        }
    }

    private void CreateCommingSoon()
    {
        GameObject buttonObj = Instantiate(ButtonPrefab, transform);
        buttonObj.name = "upcomming";
        // Get the Button component of the created UI button
        Button buttonComponent = buttonObj.GetComponent<Button>();

        // Set the text of the button as desired
        TextMeshProUGUI buttonText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Coming Soon";
    }

    private void CreateUIButton(ChapterSO scriptableObject)
    {
        // Instantiate the button prefab
        GameObject buttonObj = Instantiate(ButtonPrefab, transform);

        // Set the name of the button using the ScriptableObject's name
        buttonObj.name = scriptableObject.name;

        // Get the Button component of the created UI button
        Button buttonComponent = buttonObj.GetComponent<Button>();

        // Set the text of the button as desired
        TextMeshProUGUI buttonText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = scriptableObject.name.Replace("-", " ");

        // Add an onClick event listener to the button
        buttonComponent.onClick.AddListener(() => ButtonClicked(scriptableObject));
    }

    private void ButtonClicked(ChapterSO scriptableObject)
    {
        MissionManager.Instance.SetChapter(scriptableObject);
        ScenesManager.Instance.SetScene("StoryChapter");
    }
}
