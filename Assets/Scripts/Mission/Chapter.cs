using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour
{
    private const string SceneToLoad = "StoryChapterPopup";
    public GameObject SpritePrefab;

    ChapterSO CurrentChapter;
    void Start()
    {
        CurrentChapter = MissionManager.Instance.CurrentChapter;

        RectTransform parentRect = GetComponent<RectTransform>();
        List<MissionSO> missions = MissionManager.Instance.GetMissionsByChapterId(CurrentChapter.MissionType, CurrentChapter.Id);

        foreach (MissionSO missionSO in missions)
        {
            //Temp
            Vector2 randomPosition = new(Random.Range(-parentRect.rect.width / 2, parentRect.rect.width / 2),
                                                 Random.Range(-parentRect.rect.height / 2, parentRect.rect.height / 2));

            GameObject newMissionBtn = Instantiate(SpritePrefab, transform);
            RectTransform spriteRect = newMissionBtn.GetComponent<RectTransform>();
            spriteRect.anchoredPosition = randomPosition;

            TextMeshProUGUI buttonText = spriteRect.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = missionSO.NumInChapter.ToString();

            Button buttonComponent = newMissionBtn.GetComponent<Button>();

            // Add an onClick event listener to the button
            buttonComponent.onClick.AddListener(() => ButtonClicked(missionSO));
        }
    }

    private void ButtonClicked(MissionSO scriptableObject)
    {
        Debug.Log("Button clicked: " + scriptableObject.Name);


        // Set the new mission
        MissionManager.Instance.SetMission(scriptableObject);

        // Load the new popup scene
        ScenesManager.Instance.SetScene(SceneToLoad);

    }
}
