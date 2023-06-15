using System.Collections;
using UnityEngine;

public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.ResetSelectedEnemies();
        BattleSystem.SetSkillButtonsActive(false);
        BattleSystem.DialogueText.text = "Enemy " + BattleSystem.EnemyPlayingID + "turn";
        BattleSystem.Enemies[BattleSystem.EnemyPlayingID].atkBar = 0;
        //BattleSystem.Player.TakeDamage(BattleSystem.Enemies[0].Attack);
        //BattleSystem.Player.ApplyEffect(new SILENCE(4,BattleSystem.Player));

        yield return new WaitForSeconds(1f);

        BattleSystem.SetState(new CheckTurn(BattleSystem));
    }

    public override IEnumerator Attack()
    {
        yield break;
    }
    //TODO:Enemy Attack methode to implement (AI later)
}