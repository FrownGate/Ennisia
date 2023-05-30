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
    

        BattleSystem.dialogueText.text = "Select a spell";
        Debug.Log("You choose : " + BattleSystem.Allies[0].Skills[_spellNumber].FileName);
        Debug.Log("Number : " + _spellNumber);
        BattleSystem.SetState(new SelectTarget(BattleSystem));
        yield break;
    }
}