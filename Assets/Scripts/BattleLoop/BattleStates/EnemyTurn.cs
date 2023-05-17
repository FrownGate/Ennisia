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
            if (BattleSystem.Enemy.battleHp <= 0)
            {
                
            }
            
            BattleSystem.Player.TakeDamage(BattleSystem.Enemy.damage);
            if (BattleSystem.Player.IsDead)
            {
                BattleSystem.SetState(new Lost(BattleSystem));
            }
            else
            {
                
            }
            yield return new WaitForSeconds(1.5f);
        }
        
        //TODO:Enemy Attack methode to implement (AI later)
    }
}