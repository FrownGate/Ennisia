using UnityEngine;

public class SummonButton : SceneButton
{
    [SerializeField] private int _pullAmount;

    protected override void OnMouseUpAsButton()
    {
        if (!SummonManager.Instance.CanPull(_pullAmount)) return;
        base.OnMouseUpAsButton();
    }
}