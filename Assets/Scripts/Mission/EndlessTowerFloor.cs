using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EndlessTowerFloor : MonoBehaviour
{
    public MissionSO FloorSO;


    private void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = GetComponent<RectTransform>().sizeDelta;
    }
    private void OnMouseDown()
    {
        MissionManager.Instance.SetMission(FloorSO);
    }
}
