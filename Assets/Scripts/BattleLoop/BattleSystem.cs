using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class BattleSystem : StateMachine
{
    [SerializeField] private GameObject _firstSupport;
    [SerializeField] private GameObject _secondSupport;
    [SerializeField] private GameObject[] _skillsButtons;
    //TODO -> Move serialized ui elements in BattleSystem GameObject prefab

    //UI
    public TextMeshProUGUI DialogueText;
    public GameObject WonPopUp;
    public GameObject LostPopUp;
    public Transform PlayerStation;
    public Transform EnemyStation;

    public bool PlayerHasWin { get; private set; }
    public bool Selected { get; set; }
    public int Turn { get; set; }

    //public Entity Player { get; private set; }
    //->To access the player data, for the moment use Player.data you want access 
    public Entity Player { get; private set; }
    public List<Entity> Enemies { get; private set; }
    public List<Entity> Targetables { get; private set; }

    private int _maxEnemies => 1; //Is it used ?

    public void Start()
    {
        InitBattle();
    }

    //Init all battle elements -> used on restart button
    public void InitBattle()
    {
        Enemies = new();
        Targetables = new();

        LostPopUp.SetActive(false);
        WonPopUp.SetActive(false);

        PlayerHasWin = false;
        Selected = false;
        Turn = 0;

        GameObject enemyPrefab = GameObject.FindGameObjectWithTag("Enemy"); //TODO -> use serialized field
        enemyPrefab.GetComponent<EnemyController>().InitEntity(); //TODO -> use serialized field
        Enemies.Add(enemyPrefab.GetComponent<EnemyController>().Entity); //TODO -> use serialized field

        GameObject playerPrefab = GameObject.FindGameObjectWithTag("Player"); //TODO -> use serialized field
        playerPrefab.GetComponent<PlayerController>().InitEntity(); //TODO -> use serialized field
        Player = (Player)playerPrefab.GetComponent<PlayerController>().Entity; //TODO -> use serialized field

        SetSkillButtonsActive(true);

        foreach (var skill in Player.Skills)
        {
            skill.ConstantPassive(Enemies, Player, 0); // constant passive at battle start
        }

        SetState(new WhoGoFirst(this));
        //SimulateBattle();
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
        foreach (GameObject button in _skillsButtons)
        {
            button.SetActive(isActive);
        }
    }

    public void OnMouseUp()
    {
        if (!IsBattleOver())
        {
            Selected = true;
            GetSelectedEnemies(Enemies);
            StartCoroutine(State.Attack());
        }
    }

    public void RemoveDeadEnemies()
    {
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            if (Enemies[i].IsDead) Enemies.RemoveAt(i);
        }
    }

    public void GetSelectedEnemies(List<Entity> enemies)
    {
        Targetables.Clear();

        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.HaveBeenTargeted())
            {
                Targetables.Add(enemy);
            }
        }
    }

    public Skill GetSelectedSkill(int buttonId)
    {
        if (buttonId < 0 || buttonId >= Player.Skills.Count)
        {
            Debug.LogError("Invalid button ID");
            return null;
        }
        return Player.Skills[buttonId];
    }

    public void ResetSelectedEnemies()
    {
        foreach (var enemy in Enemies)
        {
            enemy.ResetTargetedState();
        }
    }

    private void SimulateBattle()
    {
        SetState(new AutoBattle(this));
    }

    public bool PlayerIsDead()
    {
        return Player.IsDead;
    }

    public bool AllEnemiesDead()
    {
        //Check Enemies.Count ?
        PlayerHasWin = Enemies.All(enemy => enemy.IsDead);
        return PlayerHasWin;
    }

    public bool IsBattleOver()
    {
        return PlayerIsDead() || AllEnemiesDead();
    }
}