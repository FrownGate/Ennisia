using System;
using PlayFab.GroupsModels;
using System.Collections.Generic;
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
        _quests = QuestManager.Instance.AllQuests;

        _buttons = _generator.GenerateButtonsInSlider(_quests.Count);
        var buttonIndex = 0;
        foreach (var go in _buttons)
        {
            var quest = _quests[buttonIndex];
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
                finishBtn.onClick.AddListener(() => OnClickButton(quest,go)); 
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
    
    public void OnClickButton(QuestSO quest, GameObject go)
    {
        Debug.Log("Button Finish Clicked!");
        quest.GiveRewards();
        var finishBtn = go.GetComponentInChildren<Button>();
        finishBtn.interactable = false;
        
    }
}