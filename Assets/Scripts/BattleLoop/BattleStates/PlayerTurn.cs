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
            
            yield break;
        }

        public override IEnumerator Attack()
        {
            BattleSystem.Enemy.TakeDamage(BattleSystem.Player.damage);
            
            yield return new WaitForSeconds(1f);

            if (BattleSystem.Enemy.IsDead)
            {
                BattleSystem.SetState(new Won(BattleSystem));
            }
            else
            {
                BattleSystem.SetState(new EnemyTurn(BattleSystem));
            }
        }
        
    }
}