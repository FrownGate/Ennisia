using System.Collections;
using UnityEngine;

public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        if (BattleSystem.PlayerIsDead())
        {
            BattleSystem.SetState(new Lost(BattleSystem));
            yield break;
        }
        
        BattleSystem.Turn += 1;
        BattleSystem.SetSkillButtonsActive(true);
        BattleSystem.DialogueText.text = "Your turn";
        yield return new WaitForSeconds(1.0f);
    }
}