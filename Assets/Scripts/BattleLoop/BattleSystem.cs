using System;
using System.Collections;
using System.Collections.Generic;
using BattleLoop.BattleStates;
using UnityEngine;

public class BattleSystem : StateMachine
{


    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
    // Start is called before the first frame update


    public GameObject playerPrefab;
    public GameObject enemyPrefab;


    public Entity PlayerData { get; private set; }
    public Entity EnemyData { get; private set; }   

    public BattleState state;

    private void Start()
    {
        SetupBattle();
        SetState(new WhoGoFirst(this));
    }

    private void SetupBattle()
    {
        //Retrieve player's data & enemy's data 
        PlayerData = playerPrefab.GetComponent<Entity>();
        EnemyData = enemyPrefab.GetComponent<Entity>();
        

    }


    IEnumerator PlayerAttack()
    {

        
        Debug.Log("You attacjed for  " + PlayerData.damage);
        yield return new WaitForSeconds(2);

        if(EnemyData.IsDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }


    IEnumerator EnemyTurn()
    {
        Debug.Log(EnemyData.name + " Attacked for " + EnemyData.damage);

        yield return new WaitForSeconds(2);
        

        if (!PlayerData.IsDead)
        {
            state = BattleState.PLAYERTURN;
           PlayerTurn();
        }
        else
        {
            state = BattleState.LOST;
            EndBattle();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            Debug.Log("You won");
        }
        else if(state == BattleState.LOST)
        {
            Debug.Log("You were defeated");
        }
    }
    void PlayerTurn()
    {
        Debug.Log("Choose an action ");
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerAttack());
    }

}
