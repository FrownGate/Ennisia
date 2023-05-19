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

    
    public Spell[] Spells;

    private int _selectedSpell;
    private int _selectedEnnemy;
    private int _enemyTurn;


    //UI
    public TextMeshProUGUI dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    
    
    public List<Entity> _enemyList;
    private void Start()
    {
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
        _selectedSpell = id;
        Debug.Log("Selected spell : " + id);
    }
    public void OnAttackButton(int id)
    {
        _selectedEnnemy = id;
        StartCoroutine(State.Attack());
    }

}
