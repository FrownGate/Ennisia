using System.Collections.Generic;
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
        if (BattleSystem.Player.Effects.Find(effect => effect.GetType() == typeof(Taunt)) != null)
        {
            Entity caster = BattleSystem.Player.Effects.Find(effect => effect.GetType() == typeof(Taunt)).Caster;
            List<Entity> targets = new() { caster };
            BattleSystem.ToggleSkills(false);
            BattleSystem.Player.Skills[0].Use(targets, BattleSystem.Player, BattleSystem.Turn, null);
            BattleSystem.SetState(new CheckTurn(BattleSystem));
        }
        else
        {
            BattleSystem.ToggleSkills(true);
            BattleSystem.DialogueText.text = "Your turn";
            yield return new WaitForSeconds(1.0f);
        }
    }
}