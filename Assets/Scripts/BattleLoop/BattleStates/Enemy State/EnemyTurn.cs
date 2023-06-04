using System.Collections;
using UnityEngine;

public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.ResetSelectedEnemies();
        BattleSystem.SetSkillButtonsActive(false);
        BattleSystem.DialogueText.text = "Enemy turn";
        BattleSystem.Allies[0].TakeDamage(BattleSystem.Enemies[0].Attack);

        yield return new WaitForSeconds(0.5f);

        BattleSystem.SetState(new PlayerTurn(BattleSystem));
    }

    public override IEnumerator Attack()
    {
        yield break;
    }
    //TODO:Enemy Attack methode to implement (AI later)
}