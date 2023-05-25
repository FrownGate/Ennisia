using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    public Button button;
    public QuestManager questManager;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        questManager.OnButtonPress();
    }
}
