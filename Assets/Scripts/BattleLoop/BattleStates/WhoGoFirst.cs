using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoGoFirst : State
{
    private List<Entity> _enemiesList;
    private Entity _player;

    public WhoGoFirst(BattleSystem battleSystem) : base(battleSystem)
    {
        _enemiesList = BattleSystem.Enemies;
        _player = BattleSystem.Player;
    }

    public override IEnumerator Start()
    {
        CompareSpeed();
        //BattleSystem.SetState(new PlayerTurn(BattleSystem));
        yield return new WaitForSeconds(1.5f);
    }

    private void CompareSpeed()
    {
        bool playerFirst = false; //Used ?
        int numberOfFasterEnemies = 0;
        Entity fastestEnemy = null;

        foreach (var enemy in _enemiesList)
        {
            if (enemy.Stats[Item.AttributeStat.Speed].Value > _player.Stats[Item.AttributeStat.Speed].Value)
            {
                numberOfFasterEnemies++;
            }
            if (fastestEnemy == null || fastestEnemy.Stats[Item.AttributeStat.Speed].Value < enemy.Stats[Item.AttributeStat.Speed].Value)
            {
                fastestEnemy = enemy;
                BattleSystem.EnemyPlayingID = _enemiesList.IndexOf(enemy);
            }
        }

        playerFirst = numberOfFasterEnemies == 0 ? true : false;
        State state = playerFirst ? new PlayerTurn(BattleSystem) : new EnemyTurn(BattleSystem);
        BattleSystem.SetState(state);

        //TO DO: ATB system in order to decide who start 
    }
}