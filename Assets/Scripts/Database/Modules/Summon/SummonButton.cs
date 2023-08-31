using UnityEngine;

public class SummonButton : SceneButton
{
    [SerializeField] private int _pullAmount;

    protected override void OnMouseUpAsButton()
    {
        if (!PlayFabManager.Instance.CanPull(_pullAmount)) return;
        AudioManager.Instance.Play("SFX SummonButton");
        base.OnMouseUpAsButton();
    }
}