﻿using System.Collections;
using UnityEngine;

public class SelectSpell : PlayerTurn
{
    protected int ButtonId;

    public SelectSpell(BattleSystem battleSystem, int buttonId) : base(battleSystem)
    {
        ButtonId = buttonId;
    }

    public override IEnumerator Start()
    {
        Skill selectedSkill = BattleSystem.GetSelectedSkill(ButtonId);
        BattleSystem.dialogueText.text = "Select a spell";
        Debug.Log("You choose : " + BattleSystem.Allies[0].Skills[ButtonId].FileName);
        Debug.Log("Number : " + ButtonId);
        yield return new WaitForSeconds(1.0f);
        BattleSystem.SetState(new SelectTarget(BattleSystem,selectedSkill ));
    }
}