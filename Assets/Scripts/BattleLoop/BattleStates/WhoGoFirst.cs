using System.Collections;

namespace BattleLoop.BattleStates
{
    public class WhoGoFirst : State
    {
        public WhoGoFirst(BattleSystem battleSystem) : base(battleSystem)
        {
            CompareSpeed();
        }
        
        public void CompareSpeed()
        {
            //TO DO: ATB system in order to decide who start 
            if (BattleSystem.PlayerData.speed > BattleSystem.EnemyData.speed)
            {
                BattleSystem.SetState(new PlayerTurn(BattleSystem));
            }else BattleSystem.SetState(new EnemyTurn(BattleSystem));
        }
    }
}