using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DynamicButtonGenerator))]
public class ShowQuests : MonoBehaviour
{
    private DynamicButtonGenerator _generator;
    private List<GameObject> _buttons;
    private List<QuestSO> _quests;

    [SerializeField] private Slider _questTotalCompletionSlider;
    [SerializeField] private float _totalCompletionPercentage;
    [SerializeField] private Button _achievmentButton;
    
    private void Start()
    {
        _generator = GetComponent<DynamicButtonGenerator>();
        _achievmentButton.Select();
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
        _totalCompletionPercentage = 0;
        // Split quests into three groups based on status
        var completedQuests = sortedQuests.Where(quest => quest.Completed && !quest.RewardGiven).ToList();
        var inProgressQuests = sortedQuests.Where(quest => !quest.Completed && !quest.RewardGiven).ToList();
        var rewardGivenQuests = sortedQuests.Where(quest => quest.RewardGiven).ToList();

        // Concatenate the three groups in the desired order
        var orderedQuests = completedQuests.Concat(inProgressQuests).Concat(rewardGivenQuests).ToList();

        _buttons = _generator.GenerateButtonsInSlider(orderedQuests.Count);
        var buttonIndex = 0;
        foreach (var go in _buttons)
        {
            var quest = orderedQuests[buttonIndex];
            go.name = quest.name;

            var questText = go.GetComponentInChildren<TextMeshProUGUI>();
            questText.text = $"{quest.Information.Name} :";

            var questDescription = questText.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            questDescription.text = $"{quest.Information.Description}";

            var questGoal = questDescription.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            foreach (var goal in quest.Goals) questGoal.text = $"{goal.CurrentAmount} / {goal.RequiredAmount}";

            if (quest.RewardGiven || !quest.Completed) // Hide the button if the quest is not completed and the reward is given
            {
                var finishBtn = go.GetComponentInChildren<Button>();
                finishBtn.gameObject.SetActive(false);
                if (quest.RewardGiven)
                {
                    go.GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
                }
            }
            else
            {
                var finishBtn = go.GetComponentInChildren<Button>();
                finishBtn.onClick.AddListener(() => OnClickButton(quest, go));
                finishBtn.interactable = true;
            }
            
            IndividualQuestCompletion(quest, go);

            buttonIndex++;
        }

        _totalCompletionPercentage = _totalCompletionPercentage / _buttons.Count;
        _questTotalCompletionSlider.value = _totalCompletionPercentage;
    }

    private void IndividualQuestCompletion(QuestSO quest, GameObject go)
    {

        // Calculate the quest completion percentage
        float completionPercentage = 0f;
        if (quest.Goals.Count > 0)
        {
            float totalRequiredAmount = quest.Goals.Sum(goal => goal.RequiredAmount);
            float totalCurrentAmount = quest.Goals.Sum(goal => goal.CurrentAmount);
            completionPercentage = totalCurrentAmount / totalRequiredAmount;
        }

        // Set the slider value
        var completionSlider = go.GetComponentInChildren<Slider>();
        if (completionSlider != null)
            completionSlider.value = completionPercentage;
        _totalCompletionPercentage += completionPercentage;
    }


    private void OnClickButton(QuestSO quest, GameObject go)
    {
        Debug.Log("Button Finish Clicked!");
        quest.GiveRewards();
        var finishBtn = go.GetComponentInChildren<Button>();
        finishBtn.interactable = false;
        CreateQuestInfo();
    }
}