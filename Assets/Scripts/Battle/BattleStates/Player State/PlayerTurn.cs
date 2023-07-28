﻿using System.Collections;
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
        BattleSystem.ToggleSkills(true);

        BattleSystem.UpdateEntityEffects(BattleSystem.Player);

        BattleSystem.DialogueText.text = "Your turn";
        yield return new WaitForSeconds(1.0f);
    }
}