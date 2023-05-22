using System;
using System.Collections;
using System.Collections.Generic;
using BattleLoop.BattleStates;
using UnityEngine;
using TMPro;
public class BattleSystem : StateMachine
{
    
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Entity PlayerData { get; private set; }
    public Entity EnemyData { get; private set; }

    public List<Entity> Allies { get; private set; }
    public List<Entity> Enemies { get; private set; }

    public Spell[] Spells;

    public int SelectedSpell { get; private set; }
    public int SelectedEnemy { get; private set; }
    //private int _enemyTurn;


    //UI
    public TextMeshProUGUI dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    
    

    private void Start()
    {
        //
        PlayerData = playerPrefab.GetComponent<Entity>();
        EnemyData = enemyPrefab.GetComponent<Entity>();
        //Spell
        OnSpellClick.OnClick += SelectSpell;
        Entity.OnClick += OnAttackButton;

        /*SetupBattle();*/
        InitBattleHUD();
        SetState(new WhoGoFirst(this));
    }


    private void OnDestroy()
    {
        OnSpellClick.OnClick-= SelectSpell;
        Entity.OnClick -= OnAttackButton;
    }



    private void InitBattleHUD()
    {
        playerHUD.SetHUD(PlayerData);
        enemyHUD.SetHUD(EnemyData);
    }
    
    public void SelectSpell(int id)
    {
        SelectedSpell = id;
        Debug.Log("Selected spell : " + id);
    }
    public void OnAttackButton(int id)
    {
        SelectedEnemy = id;
        StartCoroutine(State.Attack());
    }

    public void OnFirstSkillButton()
    {
        StartCoroutine(State.UseSpell());
    }

    public void OnSecondSkillButton()
    {
        
    }

}
