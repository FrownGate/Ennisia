using System;
using PlayFab.GroupsModels;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class ShowQuests : MonoBehaviour
{
    private DynamicButtonGenerator _generator;
    private List<GameObject> _buttons;
    private List<QuestSO> _quests;

    private void Start()
    {
        _generator = GetComponent<DynamicButtonGenerator>();
        Achievement();
    }

    public void Achievement()
    {
        _quests = QuestManager.Instance.Achievement;
        CreateQuestInfo();
    }

    public void Daily()
    {
        _quests = QuestManager.Instance.Daily;
        CreateQuestInfo();
    }

    public void Weekly()
    {
        _quests = QuestManager.Instance.Weekly;
        CreateQuestInfo();
    }

    private void CreateQuestInfo()
    {
        var sortedQuests = _quests.OrderBy(quest => quest.Information.ID).ToList();
        if (_buttons != null)
            foreach (var button in _buttons)
                Destroy(button);
        _buttons = _generator.GenerateButtonsInSlider(sortedQuests.Count);
        var buttonIndex = 0;
        foreach (var go in _buttons)
        {
            var quest = sortedQuests[buttonIndex];
            go.name = quest.name;

            var questText = go.GetComponentInChildren<TextMeshProUGUI>();
            questText.text = $"{quest.Information.Name} :";

            var questDescription = questText.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            questDescription.text = $"{quest.Information.Description}";

            var questGoal = questDescription.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            foreach (var goal in quest.Goals) questGoal.text = $"{goal.CurrentAmount} / {goal.RequiredAmount}";

            if (!quest.RewardGiven)
            {
                var finishBtn = go.GetComponentInChildren<Button>();
                finishBtn.onClick.AddListener(() => OnClickButton(quest, go));
                if (quest.Completed) finishBtn.interactable = true;
            }
            else
            {
                var bg = go.GetComponent<Image>();
                bg.color = new Color(0.63f, 0.63f, 0.63f);
            }

            buttonIndex++;
        }
    }

    private void OnClickButton(QuestSO quest, GameObject go)
    {
        Debug.Log("Button Finish Clicked!");
        quest.GiveRewards();
        var finishBtn = go.GetComponentInChildren<Button>();
        finishBtn.interactable = false;
    }
}