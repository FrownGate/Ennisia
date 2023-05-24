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
        EnemyContainer();
        
        
        SetState(new WhoGoFirst(this));
    }

    private void EnemyContainer()
    {
        Enemies = new List<Entity>();

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

                GameObject enemy = Instantiate(GameObject.FindGameObjectWithTag("Enemy"), hexagonPosition,
                    Quaternion.identity);
                Enemy tmp = new Enemy();
                tmp = enemy.GetComponent<EnemyController>()._enemy;
                Enemies.Add(tmp);

            }

        }
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
