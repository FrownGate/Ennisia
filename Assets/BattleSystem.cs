using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{


    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
    // Start is called before the first frame update


    public GameObject playerPrefab;
    public GameObject enemyPrefab;


    Entity playerData;
    Entity enemyData;
    public BattleState state;
    void Start()

    {

        state = BattleState.START;
        StartCoroutine(SetupBattle());


       
    }


    IEnumerator SetupBattle()
    {


        //instantiate enemies or player GO here
        playerData = playerPrefab.GetComponent<Entity>();
        enemyData = enemyPrefab.GetComponent<Entity>();

        Debug.Log("A fight has started against : " + enemyData.name);


        yield return new WaitForSeconds(2);
        state = BattleState.PLAYERTURN;

        Debug.Log(playerData.name + "'s turn");
        PlayerTurn();
    }


    IEnumerator PlayerAttack()
    {

        bool isDead = enemyData.TakeDamage(playerData.damage);
        Debug.Log("You attacjed for  " + playerData.damage);
        yield return new WaitForSeconds(2);

        if(isDead)
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
        Debug.Log(enemyData.name + " Attacked for " + enemyData.damage);

        yield return new WaitForSeconds(2);

        bool playerDead = playerData.TakeDamage(enemyData.damage);

        if (!playerDead)
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
