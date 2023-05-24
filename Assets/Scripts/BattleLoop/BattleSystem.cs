using System;
using System.Collections;
using System.Collections.Generic;
using BattleLoop.BattleStates;
using Entities;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BattleSystem : StateMachine
{
    public Transform PlayerStation;
    public Transform EnemyStation;
    

    public List<Entity> Allies { get; private set; }
    public List<Entity> Enemies { get; private set; }
    private int _maxEnemies => 10;
    
    
    public int ButtonId { get; private set; }
    
    //UI
    public TextMeshProUGUI dialogueText;
    

    private void Start()
    {
        //Entity

        for (int i = 0; i < _maxEnemies; i++)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            Instantiate(enemy, EnemyStation.position, Quaternion.identity);
        }
        
        SetState(new WhoGoFirst(this));
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
