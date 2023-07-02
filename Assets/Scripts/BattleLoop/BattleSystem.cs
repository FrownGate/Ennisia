using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;

public class BattleSystem : StateMachine
{
    public static event Action<BattleSystem> OnBattleSystemLoaded;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
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

    public static event Action<string> OnEnemyKilled;
    public static event Action<int> OnClickSFX;

    public bool PlayerHasWin { get; private set; }
    public bool Selected { get; set; }
    public int Turn { get; set; }

    public Entity Player { get; set; }
    public List<Entity> Enemies { get; private set; }
    public int EnemyPlayingID { get; set; }
    public List<Entity> Targetables { get; private set; }

    public AtkBarSystem AttackBarSystem { get; set; }

    private int _maxEnemies => 1; //Is it used ?

    public void Start()
    {
       // MissionManager.Instance.StartMission();
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
        Enemies.Add((Enemy)enemyPrefab.GetComponent<EnemyController>().Entity); //TODO -> use serialized field


        GameObject enemyPrefab2 = GameObject.Find("Enemyy"); //TODO -> use serialized field
        enemyPrefab2.GetComponent<EnemyController>().InitEntity(); //TODO -> use serialized field
        Enemies.Add((Enemy)enemyPrefab2.GetComponent<EnemyController>().Entity); //TODO -> use serialized field
        Debug.Log(Enemies.Count);

        GameObject playerPrefab = GameObject.FindGameObjectWithTag("Player"); //TODO -> use serialized field
        playerPrefab.GetComponent<PlayerController>().InitEntity(); //TODO -> use serialized field
        Player = (Player)playerPrefab.GetComponent<PlayerController>().Entity; //TODO -> use serialized field


        AttackBarSystem = new AtkBarSystem(Player, Enemies);
        AttackBarSystem.InitAtkBars();
        UpdateEntities();

        SetSkillButtonsActive(true);
        AssignSkillButton();

        foreach (var skill in Player.Skills)
        {
            skill.ConstantPassive(Enemies, Player, 0); // constant passive at battle start
        }

        SetState(new WhoGoFirst(this));
        // SimulateBattle();
    }

    public void OnAttackButton()
    {
        SetState(new SelectSpell(this, 0));
        OnClickSFX?.Invoke(1);
    }

    public void OnFirstSkillButton()
    {
        SetState(new SelectSpell(this, 1));
        OnClickSFX?.Invoke(2);
    }

    public void OnSecondSkillButton()
    {
        SetState(new SelectSpell(this, 2));
        OnClickSFX?.Invoke(3);
    }

    public void SetSkillButtonsActive(bool isActive)
    {
        foreach (GameObject button in _skillsButtons)
        {
            button.SetActive(isActive);
        }
    }

    public void AssignSkillButton()
    {
        for (int i = 0; i < Player.Skills.Count; i++)
        {
            Player.Skills[i].SkillButton = _skillsButtons[i].GetComponent<Button>();
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
            if (Enemies[i].IsDead)
            {
                Enemies.RemoveAt(i);
                OnEnemyKilled?.Invoke(Enemies[i].Name);
                Debug.LogWarning(Enemies[i].Name + "die lol mdr t nul");
            }
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

    public void SimulateBattle(Player player = null, List<Entity> enemies = null)
    {
        Player = player ?? Player;
        Enemies = enemies ?? Enemies;
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

    public void ReduceCooldown()
    {
        foreach (var skill in Player.Skills)
        {
            skill.Tick();
        }
    }

    public void SkillOnTurn(Skill selectedSkill)
    {
        float totalDamage = 0;
        foreach (var skill in Player.Skills)
        {
            skill.PassiveBeforeAttack(Enemies, Player, Turn);
        }
        totalDamage += selectedSkill.SkillBeforeUse(Targetables, Player, Turn);
        totalDamage += selectedSkill.Use(Targetables, Player, Turn);
        totalDamage += selectedSkill.AdditionalDamage(Targetables, Player, Turn, totalDamage);
        selectedSkill.SkillAfterDamage(Targetables, Player, Turn, totalDamage);

        foreach (var skill in Player.Skills)
        {
            skill.PassiveAfterAttack(Enemies, Player, Turn, totalDamage);
        }
    }

    public void UpdateEntities()
    {
        for (int i = 0; i < AttackBarSystem.AllEntities.Count - 1; i++)
        {
            Enemies[i] = AttackBarSystem.AllEntities[i];
        }
        Player = AttackBarSystem.AllEntities[AttackBarSystem.AllEntities.Count - 1];
    }

    public void UpdateEntitiesEffects()
    {
        foreach (var effect in Player.Effects)
        {
            effect.Tick(Player);

            if (effect.HasAlteration) effect.AlterationEffect(Player);
        }

        foreach (var enemy in Enemies)
        {
            foreach (var effect in enemy.Effects)
            {
                effect.Tick(enemy);

                if (effect.HasAlteration) effect.AlterationEffect(enemy);
            }
        }
    }
}