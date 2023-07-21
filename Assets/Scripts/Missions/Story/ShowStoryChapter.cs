using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowStoryChapter : MonoBehaviour
{
    public GameObject Info;
    public GameObject StartBtn;
    public GameObject SpritePrefab;
    public ChapterSO CurrentChapter;

    void Start()
    {
        CurrentChapter = MissionManager.Instance.CurrentChapter;

        RectTransform parentRect = GetComponent<RectTransform>();

        List<MissionSO> missions = MissionManager.Instance.GetMissionsByChapterId(CurrentChapter.MissionType, CurrentChapter.Id);

        foreach (MissionSO missionSO in missions)
        {
            //TODO -> Get mission position from SO (depending on Ennisia map)
            Vector2 randomPosition = new(
                Random.Range(-parentRect.rect.width / 2, parentRect.rect.width / 2),
                Random.Range(-parentRect.rect.height / 2, parentRect.rect.height / 2)
            );

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
        StartBtn.SetActive(true);

        MissionSO mission = MissionManager.Instance.CurrentMission;

        TextMeshProUGUI buttonText = Info.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = $"<b>{mission.Name}</b>\n";
        buttonText.text += "<b>Enemies:</b>\n";

        foreach (string enemy in mission.Enemies)
        {
            buttonText.text += $"-{enemy}\n";
        }

        buttonText.text += "<b>Rewards:</b>\n";
        foreach (var currencyReward in mission.CurrencyRewards)
        {
            buttonText.text += $"-{currencyReward.Key}: {currencyReward.Value}\n";
        }

        if (mission.RewardData != null && mission.RewardData.Count > 0)
        {
            buttonText.text += "<b>Gear:</b>\n";
            foreach (var reward in mission.RewardData)
            {
                buttonText.text += $"-{reward}\n";
            }
        }

        buttonText.text += "<b>XP:</b> " + mission.Experience;
    }
}