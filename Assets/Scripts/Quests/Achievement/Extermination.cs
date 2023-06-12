using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extermination : Quests
{
    int enemyKilled;
    public override void CheckCondition()
    {
        enemyKilled++;
        if(enemyKilled >= 100) 
        { 
            GiveRewards();
        }
    }

    private void OnEnable()
    {
        BattleSystem.OnEnemyKilled += CheckCondition;
    }
    private void OnDisable()
    {
        BattleSystem.OnEnemyKilled -= CheckCondition;
    }
}
