using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class EndlessTowerFloor : MonoBehaviour
{
    public MissionSO FloorSO;

    private void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = GetComponent<RectTransform>().sizeDelta;
        Image image = GetComponent<Image>();
        image.color = FloorSO.State switch
        {
            MissionState.Locked => Color.grey,
            _ => Color.white
        };
        
        
    }
    private void OnMouseUpAsButton()
    {
        if(this.FloorSO.State == MissionState.Locked) return;
        MissionManager.Instance.SetMission(FloorSO);
    }
}