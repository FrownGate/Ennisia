using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem) { }

    public override IEnumerator Start()
    {
        BattleSystem.dialogueText.text = "Enemy turn";

        BattleSystem.RemoveDeadEnemies();
        
        BattleSystem.Allies[0].TakeDamage(BattleSystem.Enemies[0].Damage);
        
        yield return new WaitForSeconds(0.5f);
        
        BattleSystem.SetState(new PlayerTurn(BattleSystem));
    }

    public override IEnumerator Attack()
    {
        yield break;
    }
    //TODO:Enemy Attack methode to implement (AI later)
}