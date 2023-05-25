using System.Collections;
using UnityEngine;

public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.dialogueText.text = "Enemy turn";

        /*if (BattleSystem.PlayerData.IsDead)
        {
            BattleSystem.SetState(new Lost(BattleSystem));
        }
        else
        {
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }*/

        yield return new WaitForSeconds(1.5f);
    }

    public override IEnumerator Attack()
    {
        yield break;
    }
    //TODO:Enemy Attack methode to implement (AI later)
}