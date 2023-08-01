using System.Collections;
using UnityEngine;

public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        if (BattleSystem.Player.IsDead)
        {
            BattleSystem.SetState(new Lost(BattleSystem));
            yield break;
        }

        BattleSystem.Turn += 1;

        if (BattleSystem.Player.HasEffect(new Taunt()))
        {
            Entity caster = BattleSystem.Player.GetEffect(new Taunt()).Caster;
            BattleSystem.Targets.Add(caster);
            BattleSystem.SetState(new SelectTarget(BattleSystem, BattleSystem.Player.Skills[0]));
        }
        else
        {
            BattleSystem.ToggleSkills(true);
            BattleSystem.DialogueText.text = "Your turn";
            yield return new WaitForSeconds(1.0f);
        }
    }
}