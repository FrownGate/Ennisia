using UnityEngine;

public class SummonButton : SceneButton
{
    [SerializeField] private int _pullAmount;

    protected override void OnMouseUpAsButton()
    {
        //TODO -> check summon tickets
        int cost = PlayFabManager.Instance.SummonCost * _pullAmount;
        if (!PlayFabManager.Instance.HasEnoughCurrency(cost, Currency.Crystals)) return;

        SummonManager.Instance.Amount = _pullAmount;
        base.OnMouseUpAsButton();
    }
}