using System.Collections;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class WhoGoFirst : State
    {
        public WhoGoFirst(BattleSystem battleSystem) : base(battleSystem)
        {
            
        }

        public override IEnumerator Start()
        {
            CompareSpeed();
            yield break;
        }

        public void CompareSpeed()
        {
            //TO DO: ATB system in order to decide who start 
            if (BattleSystem.Player.speed > BattleSystem.Enemy.speed)
            {
                Debug.Log("Enter PlayerTurn State");
                BattleSystem.SetState(new PlayerTurn(BattleSystem));
            }else BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }
    }
}