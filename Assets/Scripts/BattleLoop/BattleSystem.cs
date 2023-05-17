using System;
using System.Collections;
using System.Collections.Generic;
using BattleLoop.BattleStates;
using UnityEngine;

public class BattleSystem : StateMachine
{
    public GameObject playerPrefab;
    public SpellManager spellManager;
    public GameObject enemyPrefab;
    
    public Entity Player { get; private set; }
    public Entity Enemy { get; private set; }
    
    private void Start()
    {
        SetupBattle();
        SetState(new WhoGoFirst(this));
    }

    private void SetupBattle()
    {
        //instantiate enemies or player GO here
        Player = playerPrefab.GetComponent<Entity>();
        Enemy = enemyPrefab.GetComponent<Entity>();

        Debug.Log("A fight has started against : " + Enemy.name);

    }
    
    



}
