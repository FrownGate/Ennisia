using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleSystem : StateMachine
{
    [SerializeField] private GameObject Support1;
    [SerializeField] private GameObject Support2;

    [SerializeField] public GameObject[] SkillsButton;

    [SerializeField] public GameObject _wonPopUp;
    [SerializeField] public GameObject _lostPopUp;

    public Transform PlayerStation;
    public Transform EnemyStation;

    //public Entity Player { get; private set; }
    //->To access the player data, for the moment use Allies[0].data you want access 
    public List<Entity> Allies { get; private set; }
    public List<Entity> Enemies { get; private set; }
    public List<Entity> Targetables { get; private set; }
    private int _maxEnemies => 1;


    public bool _selected = false;
    public int turn = 0;

    //UI
    public TextMeshProUGUI dialogueText;

    public void Start()
    {
        Enemies = new List<Entity>();
        Targetables = new List<Entity>();
        _lostPopUp.SetActive(false);
        _wonPopUp.SetActive(false);
        //Entity
        EnemyContainer();
        InitPlayer();

        foreach (var skill in Allies[0].Skills)
        {
            skill.ConstantPassive(Enemies, Allies[0], 0); // constant passive at battle start
        }

        //SetState(new WhoGoFirst(this));
        SimulateBattle();
    }

    private void EnemyContainer()
    {
        GameObject enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");
        Enemies.Add(enemyPrefab.GetComponent<EnemyController>().Enemy);
    }

    private void InitPlayer()
    {
        Allies = new List<Entity>();
        GameObject playGo = GameObject.FindGameObjectWithTag("Player");
        Player tmp = playGo.GetComponent<PlayerController>().Player;
        Allies.Add(tmp);
    }

    public void OnAttackButton()
    {
        SetState(new SelectSpell(this, 0));
    }

    public void OnFirstSkillButton()
    {
        SetState(new SelectSpell(this, 1));
    }

    public void OnSecondSkillButton()
    {
        SetState(new SelectSpell(this, 2));
    }
    
    public void SetSkillButtonsActive(bool isActive)
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("SkillButton");

        foreach (GameObject buttonObject in buttons)
        {
            buttonObject.SetActive(isActive);
        }
    }

    public void OnMouseUp()
    {
        _selected = true;
        if (_selected)
        {
            GetSelectedEnemies(Enemies);

            StartCoroutine(State.Attack());
        }

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

    public void GetSelectedEnemies(List<Entity> enemies)
    {
        Targetables.Clear();
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
    }

    public Skill GetSelectedSkill(int buttonId)
    {
        if (buttonId < 0 || buttonId >= Allies[0].Skills.Count)
        {
            Debug.LogError("Invalid button ID");
            return null;
        }
        return Allies[0].Skills[buttonId];
    }

    public void ResetSelectedEnemies()
    {
        foreach (var enemy in Enemies)
        {
            enemy.ResetTargetedState();
        }
    }

    public void ReStart()
    {
        Enemies.Clear();
        Allies.Clear();
        Targetables.Clear();
        _selected = false;
        turn = 0;
        
        _lostPopUp.SetActive(false);
        _wonPopUp.SetActive(false);
        
        GameObject enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");
        enemyPrefab.GetComponent<EnemyController>().ResetStats();
        Enemies.Add(enemyPrefab.GetComponent<EnemyController>().Enemy);

        GameObject playerPrefab = GameObject.FindGameObjectWithTag("Player");
        playerPrefab.GetComponent<PlayerController>().ResetStats();
        Allies.Add(playerPrefab.GetComponent<PlayerController>().Player);
        
        foreach (var skill in Allies[0].Skills)
        {
            skill.ConstantPassive(Enemies, Allies[0], 0);
        }
        
        SetState(new WhoGoFirst(this));
    }

    private void SimulateBattle()
    {
        SetState(new AutoBattle(this));
    }
    
    public bool IsBattleOver()
    {
        bool allAlliesDead = true;
        bool allEnemiesDead = true;
        
        foreach (var ally in Allies)
        {
            if (ally.IsDead) continue;
            allAlliesDead = false;
            break;
        }
        
        foreach (var enemy in Enemies)
        {
            if (enemy.IsDead) continue;
            allEnemiesDead = false;
            break;
        }
        return allAlliesDead || allEnemiesDead;
    }
    
    public bool DidPlayerWin()
    {
        bool allEnemiesDead = true;
        
        foreach (var enemy in Enemies)
        {
            if (!enemy.IsDead)
            {
                allEnemiesDead = false;
                break;
            }
        }
        return allEnemiesDead;
    }
}