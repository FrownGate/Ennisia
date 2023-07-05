using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class BattleSystem : StateMachine
{
    public static event Action<BattleSystem> OnBattleLoaded;
    public static event Action OnBattleEnded;
    public static event Action<string> OnEnemyKilled;

    //UI Prefabs
    [SerializeField] private GameObject _entitySlot;
    [SerializeField] private GameObject _skillButton;
    [SerializeField] private GameObject _firstSupport;
    [SerializeField] private GameObject _secondSupport;
    //TODO -> Move serialized ui elements in BattleSystem GameObject prefab
    //TODO -> Add Supports Skills

    //UI
    public TextMeshProUGUI DialogueText;
    public GameObject WonPopUp;
    public GameObject LostPopUp;

    public bool PlayerHasWin { get; private set; }
    public int Turn { get; set; }

    public Entity Player { get; set; }
    public List<Entity> Enemies { get; private set; }
    public int EnemyPlayingID { get; set; }
    public List<Entity> Targets { get; private set; }

    public AtkBarSystem AttackBarSystem { get; set; }

    private Canvas _canvas;

    private void Awake()
    {
        OnBattleLoaded?.Invoke(this);
        _canvas = GetComponentInParent<Canvas>();
        EntityHUD.OnEntitySelected += SelectEntity;
        SkillHUD.OnSkillSelected += SelectSkill;
        InitBattle();
    }

    private void OnDestroy()
    {
        EntityHUD.OnEntitySelected -= SelectEntity;
        SkillHUD.OnSkillSelected -= SelectSkill;
    }

    //Init all battle elements -> used on restart button
    public void InitBattle()
    {
        Targets = new();

        LostPopUp.SetActive(false);
        WonPopUp.SetActive(false);

        PlayerHasWin = false;
        Turn = 0;

        InitPlayer();
        InitEnemies();

        AttackBarSystem = new AtkBarSystem(Player, Enemies);
        AttackBarSystem.InitAtkBars();
        //UpdateEntities();

        SetState(new WhoGoFirst(this));
        // SimulateBattle();
    }

    private void InitPlayer()
    {
        Player = new Player
        {
            HUD = Instantiate(_entitySlot, _canvas.transform).GetComponent<EntityHUD>()
        };

        Player.HUD.Init((Player)Player);

        int position = 0;

        foreach (var skill in Player.Skills)
        {
            //TODO -> Set position
            skill.ConstantPassive(Enemies, Player, Turn); // constant passive at battle start
            skill.Button = Instantiate(_skillButton, _canvas.transform).GetComponent<SkillHUD>();
            skill.Button.Init(skill, position);
            position += 160;
        }
    }

    private void InitEnemies()
    {
        MissionSO mission = MissionManager.Instance.CurrentMission;
        int wave = MissionManager.Instance.CurrentWave;

        //Debug.Log(mission.Name);
        //Debug.Log(wave);
        //Debug.Log(mission.Waves.Count);

        Enemies = new();

        //Enemy enemy = new();
        //Enemies.Add(enemy);
        //enemy.HUD = Instantiate(_entitySlot, _canvas.transform).GetComponent<EntityHUD>();
        //enemy.HUD.Init(enemy);
        //enemy.HUD.transform.localPosition = new Vector3(495, 0, 0);

        int id = 1;

        foreach (var enemyName in mission.Waves[wave])
        {
            Debug.Log(enemyName);
            Enemy enemy = new(id, Resources.Load<EnemySO>($"SO/Enemies/{enemyName}"))
            {
                HUD = Instantiate(_entitySlot, _canvas.transform).GetComponent<EntityHUD>()
            };

            enemy.HUD.Init(enemy);
            enemy.HUD.transform.localPosition = new Vector3(495, 0, 0); //TODO -> Change position for each

            Enemies.Add(enemy);
            id++;
        }
    }

    private void SelectSkill(Skill skill)
    {
        SetState(new SelectTarget(this, skill));
    }

    public void ToggleSkills(bool active)
    {
        foreach (var skill in Player.Skills)
        {
            skill.Button.ToggleHUD(active);
        }
    }

    //public void OnAttackButton()
    //{
    //    SetState(new SelectSpell(this, 0));
    //    OnClickSFX?.Invoke(1);
    //}

    //public void OnFirstSkillButton()
    //{
    //    SetState(new SelectSpell(this, 1));
    //    OnClickSFX?.Invoke(2);
    //}

    //public void OnSecondSkillButton()
    //{
    //    SetState(new SelectSpell(this, 2));
    //    OnClickSFX?.Invoke(3);
    //}

    private void SelectEntity(Entity entity)
    {
        Debug.Log("Entity seleted.");
        if (IsBattleOver() || State is not SelectTarget || entity.IsDead) return;
        Targets.Add(entity);
        StartCoroutine(State.Attack());
    }

    public void RemoveDeadEnemies()
    {
        foreach (var target in Targets)
        {
            if (!target.IsDead) continue;
            //TODO -> hide HUD
            OnEnemyKilled?.Invoke(target.Name);
            Debug.Log($"Killed {target.Name}");
            Enemies.Remove(target);
            AttackBarSystem.AllEntities.Remove(target);
        }
    }

    public bool AllEnemiesDead()
    {
        PlayerHasWin = Enemies.Count == 0 || Enemies.All(enemy => enemy.IsDead);
        return PlayerHasWin;
    }

    public bool IsBattleOver()
    {
        return Player.IsDead || AllEnemiesDead();
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

        totalDamage += selectedSkill.SkillBeforeUse(Targets, Player, Turn);
        totalDamage += selectedSkill.Use(Targets, Player, Turn);
        totalDamage += selectedSkill.AdditionalDamage(Targets, Player, Turn, totalDamage);

        foreach (var target in Targets) target.TakeDamage(totalDamage);

        selectedSkill.SkillAfterDamage(Targets, Player, Turn, totalDamage);

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

    public void BattleEnded(bool won)
    {
        DialogueText.text = won ? "YOU WON" : "YOU LOST";

        foreach (var skill in Player.Skills) Destroy(skill.Button);

        //foreach (var skill in Player.Skills) skill.TakeOffStats(Enemies, Player, 0); //constant passives at battle end
        foreach (var stat in Player.Stats) stat.Value.RemoveAllModifiers();
        OnBattleEnded?.Invoke();
    }

    #region AutoBattle
    public void SimulateBattle(Player player = null, List<Entity> enemies = null)
    {
        Player = player ?? Player;
        Enemies = enemies ?? Enemies;
        SetState(new AutoBattle(this));
    }

    public void GetSelectedEnemies(List<Entity> enemies)
    {
        Targets.Clear();

        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.HaveBeenTargeted())
            {
                Targets.Add(enemy);
            }
        }
    }

    public void ResetSelectedEnemies()
    {
        foreach (var enemy in Enemies)
        {
            enemy.ResetTargetedState();
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
    #endregion
}