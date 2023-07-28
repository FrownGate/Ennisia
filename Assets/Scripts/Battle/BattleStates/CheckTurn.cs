using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTurn : State
{
    private List<Entity> _enemiesList;
    private Entity _player;

    public CheckTurn(BattleSystem battleSystem) : base(battleSystem)
    {
        _enemiesList = BattleSystem.Enemies;
        _player = BattleSystem.Player;
    }

    public override IEnumerator Start()
    {
        BattleSystem.AttackBarSystem.IncreaseAtkBars();
        BattleSystem.UpdateEntities();
        CompareAttackBars();
        //BattleSystem.SetState(new PlayerTurn(BattleSystem));
        yield return new WaitForSeconds(1.5f);
    }

    private void CompareAttackBars()
    {
        //TODO -> utiliser une fonction réutilisable depuis AtkBarSystem avec une liste d'Entity à trier plutôt
        bool playerFirst = false;
        int numberOfFasterEnemies = 0;
        Entity fastestEnemy = null;

        foreach (var enemy in _enemiesList)
        {
            if (enemy.AtkBarPercentage > _player.AtkBarPercentage) numberOfFasterEnemies++;

            if (fastestEnemy == null || fastestEnemy.AtkBarPercentage < enemy.AtkBarPercentage)
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