using System.Collections;
using UnityEngine;

public class SelectSpell : PlayerTurn
{
    protected int _spellNumber;

    public SelectSpell(BattleSystem battleSystem) : base(battleSystem)
    {
        _spellNumber = battleSystem.ButtonId;
        
    }

    public override IEnumerator Start()
    {
        Debug.Log("You choose spell" + _spellNumber);
        BattleSystem.dialogueText.text = "Select a spell";
        BattleSystem.SetState(new SelectTarget(BattleSystem));
        yield break;
    }
}