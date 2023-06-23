using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTurn : State
{
    private List<Entity> _enemiesList;
    private Entity _player;
    //TODO -> replace list by Entity Player

    public CheckTurn(BattleSystem battleSystem) : base(battleSystem)
    {
        _enemiesList = BattleSystem.Enemies;
        _player = BattleSystem.Player;
    }

    public override IEnumerator Start()
    {
        BattleSystem.AttackBarSystem.IncreaseAtkBars();
        BattleSystem.UpdateEntities();
        BattleSystem.UpdateEntitiesBuffEffects();
        BattleSystem.UpdateEntitiesAlterations();
        CompareAttackBars();
        //BattleSystem.SetState(new PlayerTurn(BattleSystem));
        yield return new WaitForSeconds(1.5f);
    }

    private void CompareAttackBars()
    {
        bool playerFirst = false;
        int numberOfFasterEnemies = 0;
        Entity fastestEnemy = null;

        foreach (var enemy in _enemiesList)
        {
            if (enemy.atkBarPercentage > _player.atkBarPercentage)
            {
                numberOfFasterEnemies++;
            }
            if (fastestEnemy == null || fastestEnemy.atkBarPercentage < enemy.atkBarPercentage)
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