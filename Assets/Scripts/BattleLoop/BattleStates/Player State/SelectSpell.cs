using System.Collections;
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
        BattleSystem.DialogueText.text = "Select a spell";

        Debug.Log("You choose : " + BattleSystem.Player.Skills[ButtonId].FileName);
        Debug.Log("Number : " + ButtonId);
        
        BattleSystem.SetState(new SelectTarget(BattleSystem,selectedSkill ));
        yield break;
    }
}