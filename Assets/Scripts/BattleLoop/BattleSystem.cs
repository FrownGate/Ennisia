using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleLoop.BattleStates;
using Entities;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Update = UnityEngine.PlayerLoop.Update;

public class BattleSystem : StateMachine
{
    public Transform PlayerStation;
    public Transform EnemyStation;
    
    public Entity Player { get; private set; }
    public List<Entity> Allies { get; private set; }
    public List<Entity> Enemies { get; private set; }
    public List<Entity> Targetables { get; private set; }
    private int _maxEnemies => 1;
    
    public int ButtonId { get; private set; }
    private int _selectedTargetNumber = 2;
    public int _selected = 0;
    
    //UI
    public TextMeshProUGUI dialogueText;
    

    private void Start()
    {
        Player = new Player();
        Enemies = new List<Entity>();
        Targetables = new List<Entity>();
        //Entity
        EnemyContainer();
        InitPlayer();
        
        SetState(new WhoGoFirst(this));
    }

    private void Update()
    {
        if (_selected == _selectedTargetNumber)
        {
            Targetables = GetSelectedEnemies(Enemies);
            StartCoroutine(State.Attack());
            Targetables.Clear();
        }
    }

    private void LateUpdate()
    {

    }

    private void EnemyContainer()
    {
        GameObject enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");
        Vector3 gridCenter = EnemyStation.position;
        int numRows = 5;
        int numColumns = 5;
        float hexagonSize = 1f;
        float hexagonSpacing = 1f;
        
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                // Calculate position based on row and column index
                float xPos = col * (hexagonSize + hexagonSpacing);
                float yPos = row * (hexagonSize + hexagonSpacing) * Mathf.Sqrt(3);
                if (row % 2 == 1) // Offset every other row
                    xPos += (hexagonSize + hexagonSpacing) / 2f;

                Vector3 hexagonPosition = gridCenter + new Vector3(xPos, yPos, 0f);

                GameObject enemyInstance = Instantiate(enemyPrefab, hexagonPosition, Quaternion.identity);
                EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
                Enemy tmp = enemyController._enemy;
                Enemies.Add(tmp);
            }
        }
    }

    private void InitPlayer()
    {
        Allies = new List<Entity>();
        GameObject playGo = GameObject.FindGameObjectWithTag("Player");
        Player tmp = playGo.GetComponent<PlayerController>()._player;
        Allies.Add(tmp);
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
        _selected++;
        Debug.Log("You selected:" + _selected +"target");
    }

    public void NextTurn()
    {
        while (!IsEmpty(Enemies))
        {
            SetState(new EnemyTurn(this));
        }
        
        SetState(new Won(this));
    }

    public bool IsEmpty<T>(List<T> list)
    {
        if (list == null)
        {
            return true;
        }
        return !list.Any();
    }
    
    public void RemoveDeadEnemies()
    {
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            if (Enemies[i].CurrentHp <= 0)
            {
                Enemies.RemoveAt(i);
            }
        }
    }
    
    private List<Entity> GetSelectedEnemies(List<Entity> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.HaveBeTargeted())
            {
                if (enemy != null && enemy.HaveBeTargeted())
                {
                    Targetables.Add(enemy);
                }
            }
        }
        return Targetables;
    }

}
