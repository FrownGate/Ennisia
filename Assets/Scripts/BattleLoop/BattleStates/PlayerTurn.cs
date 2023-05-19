using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class PlayerTurn : State
    {
        public bool nextRound;
        public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
        {
            nextRound = false;
        }

        public override IEnumerator Start()
        {
            BattleSystem.dialogueText.text = "Your turn";
            
            
            Debug.Log("Choose an action..");
            yield break;
        }

        public override IEnumerator Attack()
        {
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