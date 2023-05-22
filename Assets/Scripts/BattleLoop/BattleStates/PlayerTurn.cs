using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class PlayerTurn : State
    {
        public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            BattleSystem.dialogueText.text = "Your turn";

            
            yield return new WaitForSeconds(1.0f);
            BattleSystem.dialogueText.text = "Choose a spell";
            
            BattleSystem.SetState(new SelectSpell(BattleSystem));
        }

        public override IEnumerator Attack()
        {
            /*foreach (var enemy in BattleSystem.Enemies)
            {
                if (enemy.battleId == BattleSystem.SelectedEnemy)
                {
                    enemy.TakeDamage(BattleSystem.PlayerData.damage);
                    BattleSystem.enemyHUD.SetHp(enemy.currentHp);
                    break;
                }
            }*/
            
            
            BattleSystem.EnemyData.TakeDamage(BattleSystem.PlayerData.damage);
            BattleSystem.enemyHUD.SetHp(BattleSystem.EnemyData.currentHp);
            
            if (BattleSystem.EnemyData.IsDead)
            {
                BattleSystem.SetState(new Won(BattleSystem));
            }
            else
            {
                BattleSystem.SetState(new EnemyTurn(BattleSystem));
            }
            yield return new WaitForSeconds(1f);
        }
        
    }
}