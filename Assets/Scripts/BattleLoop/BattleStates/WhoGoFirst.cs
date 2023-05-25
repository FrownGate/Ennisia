using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace BattleLoop.BattleStates
{
    public class WhoGoFirst : State
    {
        private List<Entity> _enemiesList;
        private List<Entity> _playerList;
        public WhoGoFirst(BattleSystem battleSystem) : base(battleSystem)
        {
            _enemiesList = BattleSystem.Enemies;
            _playerList = BattleSystem.Allies;
        }

        public override IEnumerator Start()
        {
            CompareSpeed();
            //BattleSystem.SetState(new PlayerTurn(BattleSystem));
            yield return new WaitForSeconds(1.5f);
        }

        private void CompareSpeed()
        {
            float enemiesSpeed = 0;
            float playerSpeed = 0;
            foreach (var enemy in _enemiesList)
            {
                enemiesSpeed += enemy.Speed;
            }

            foreach (var ally in _playerList)
            {
                playerSpeed += ally.Speed;
            }

            if (playerSpeed > enemiesSpeed) { BattleSystem.SetState(new PlayerTurn(BattleSystem));}
            else {BattleSystem.SetState(new EnemyTurn(BattleSystem));}
          
            //TO DO: ATB system in order to decide who start 
  
        }
    }
}