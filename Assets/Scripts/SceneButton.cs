using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneButton : MonoBehaviour
{
    [Scene] public string Scene;
    [SerializeField] private string _params;

    private void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = GetComponent<RectTransform>().sizeDelta;
    }

    private void OnMouseDown()
    {
        ScenesManager.Instance.SetScene($"{Scene}#{_params}");
        switch (Scene)
        {
            case "Raids":
                MissionManager.Instance.StartMission(MissionManager.MissionType.Raid,1 );
                break;
            case "MainStory":
                MissionManager.Instance.StartMission(MissionManager.MissionType.MainStory, 1);
                break;
        }
    }
}