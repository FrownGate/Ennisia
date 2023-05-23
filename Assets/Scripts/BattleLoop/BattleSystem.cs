using System;
using System.Collections;
using System.Collections.Generic;
using BattleLoop.BattleStates;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BattleSystem : StateMachine
{
    public Transform PlayerStation;
    public Transform EnemyStation;
    
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Entity PlayerData { get; private set; }
    public Entity EnemyData { get; private set; }

    public List<Entity> Allies { get; private set; }
    public List<Entity> Enemies { get; private set; }
    private int _maxEnemies => 10;

    public Spell[] Spells;
    
    public int ButtonId { get; private set; }
    
    //UI
    public TextMeshProUGUI dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    

    private void Start()
    {
        //Entity
        PlayerData = playerPrefab.GetComponent<Entity>();
        EnemyData = enemyPrefab.GetComponent<Entity>();

        Enemies = new List<Entity> { EnemyData };

        /*SetupBattle();*/
        InitBattleHUD();
        SetState(new WhoGoFirst(this));
    }

    private void Update()
    {
        
    }

    private void InitBattleHUD()
    {
        playerHUD.SetHUD(PlayerData);
        enemyHUD.SetHUD(EnemyData);
    }
    
    public void OnAttackButton()
    {
        ButtonId = 0;
        SetState(new SelectSpell(this));
    }

    public void OnFirstSkillButton()
    {
        ButtonId = 1;
        SetState(new SelectSpell(this));
    }

    public void OnSecondSkillButton()
    {
        ButtonId = 2;
        SetState(new SelectSpell(this));
    }

    public void OnMouseUp()
    {
        StartCoroutine(State.Attack());
    }

}
