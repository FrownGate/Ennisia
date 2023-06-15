using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoGoFirst : State
{
    private List<Entity> _enemiesList;
    private List<Entity> _playerList;
    //TODO -> replace list by Entity Player

    public WhoGoFirst(BattleSystem battleSystem) : base(battleSystem)
    {
        _enemiesList = BattleSystem.Enemies;
        _playerList = new List<Entity> { BattleSystem.Player };
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
            enemiesSpeed += enemy.Stats[Item.AttributeStat.Speed].Value;
        }

        foreach (var ally in _playerList)
        {
            playerSpeed += ally.Stats[Item.AttributeStat.Speed].Value;
        }

        State state = playerSpeed > enemiesSpeed ? new PlayerTurn(BattleSystem) : new EnemyTurn(BattleSystem);
        BattleSystem.SetState(state);

        //TO DO: ATB system in order to decide who start 
    }
}