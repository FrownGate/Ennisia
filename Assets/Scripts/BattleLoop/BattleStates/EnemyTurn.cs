using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class EnemyTurn : State
    {
        public EnemyTurn(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("Enemy dead");
            if (BattleSystem.EnemyData.currentHp <= 0)
            {
                Debug.Log("Enemy dead");
                BattleSystem.SetState(new Won(BattleSystem));
            }
            
            BattleSystem.PlayerData.TakeDamage(BattleSystem.EnemyData.damage);
            BattleSystem.playerHUD.SetHp(BattleSystem.PlayerData.currentHp);
            
            if (BattleSystem.PlayerData.IsDead)
            {
                BattleSystem.SetState(new Lost(BattleSystem));
            }
            else
            {
                BattleSystem.SetState(new PlayerTurn(BattleSystem));
            }
            yield return new WaitForSeconds(1.5f);
        }

        public override IEnumerator Attack()
        {
            
            
            yield break;
        }
        //TODO:Enemy Attack methode to implement (AI later)
    }
}