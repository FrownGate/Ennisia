using System;
using System.Collections;
using System.Collections.Generic;
using BattleLoop.BattleStates;
using UnityEngine;

public class BattleSystem : StateMachine
{


    public enum BattleState { START, PLAYERTURN, ENEMYSELECTION, ENEMYTURN, WON, LOST }
    // Start is called before the first frame update

    
    public GameObject playerPrefab;
    public GameObject enemyPrefab;


    public Spell[] Spells;

    private int _selectedSpell;
    private int _selectedEnnemy;

    private int _enemyTurn;

    public Entity PlayerData { get; private set; }
    public Entity EnemyData { get; private set; }

    public BattleState state;


    public List<Entity> _enemyList;
    private void Start()
    {
        OnSpellClick.OnClick += SelectSpell;
        Entity.OnClick += OnAttackButton;
        SetupBattle();
        SetState(new WhoGoFirst(this));
    }
    private void OnDestroy()
    {
        OnSpellClick.OnClick-= SelectSpell;
        Entity.OnClick -= OnAttackButton;
    }

    private void SetupBattle()
    {
        //instantiate enemies or player GO here
        _selectedSpell = 0;
        _selectedEnnemy = 0;
        _enemyTurn = 0;

        PlayerData = playerPrefab.GetComponent<Entity>();
        EnemyData = enemyPrefab.GetComponent<Entity>();


        for(int i = 0;i < _enemyList.Count;i++)
        {
            _enemyList[i].battleId = i;
        }

        

        Debug.Log("A fight has started against : " + EnemyData.name);
        state = BattleState.PLAYERTURN;
        Debug.Log(PlayerData.name + "'s turn");


        PlayerTurn();
    }


    IEnumerator PlayerAttack()
    {
        state = BattleState.ENEMYTURN;
        int damage = Spells[_selectedSpell].damage;

        Debug.Log("You attacjed for  " + damage);
        _enemyList[_selectedEnnemy].TakeDamage(damage);
        yield return new WaitForSeconds(2);

        if (_enemyList[_selectedEnnemy].IsDead)
        {
            _enemyList[_selectedEnnemy] = null;
            BattleCheck();
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }


    IEnumerator EnemyTurn()
    {
        Entity currentEnemy = _enemyList[_enemyTurn];
        Debug.Log(currentEnemy.name + " Attacked for " + currentEnemy.damage);

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


    private void BattleCheck()
    {
        int count = 0;
        for(int i = 0; i < _enemyList.Count; i++)
        {
            if (_enemyList[i] == null)
            {
                count++;
            }
        }

        if(count == _enemyList.Count)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }
    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            Debug.Log("You won");
        }
        else if (state == BattleState.LOST)
        {
            Debug.Log("You were defeated");
        }
    }
     private void PlayerTurn()
    {
        Debug.Log("Choose a spell and enemy ");
    }

    public void SelectSpell(int id)
    {
        _selectedSpell = id;
        Debug.Log("Selected spell : " + id);
    }
    public void OnAttackButton(int id)
    {
        if (state != BattleState.PLAYERTURN) return;
        _selectedEnnemy = id;
        StartCoroutine(PlayerAttack());
    }

}
