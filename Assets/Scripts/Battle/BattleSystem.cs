using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;

public class BattleSystem : StateMachine
{
    public static event Action<BattleSystem> OnBattleLoaded;
    public static event Action OnWaveCompleted;
    public static event Action OnBattleCompleted;
    public static event Action<bool> OnPlayerLose;
    public static event Action<BattleSystem> OnSimulationStart;
    public static event Action<string> OnEnemyKilled;

    //UI
    [Header("UI")]
    [SerializeField] private GameObject _supportSlot;
    [SerializeField] private GameObject _entitySlot;
    [SerializeField] private GameObject _skillButton;
    [SerializeField] private GameObject _skillSlot;
    [SerializeField] private Canvas _canvasPC;
    [SerializeField] private Canvas _canvasMobile;
    [SerializeField] private Image _background;
    public TextMeshProUGUI DialogueText;
    public GameObject WonPopUp;
    public GameObject LostPopUp;
    //TODO -> move popup in BattleSystem game object

    public bool PlayerHasWin { get; private set; }
    public int Turn { get; set; }

    public Entity Player { get; set; }
    public List<Entity> Enemies { get; private set; }
    public List<Entity> Allies { get; private set; }
    public int EnemyPlayingID { get; set; }
    public List<Entity> Targets { get; private set; }

    public AtkBarSystem AttackBarSystem { get; set; }

    private Canvas _canvas;

    private void Awake()
    {
        OnBattleLoaded?.Invoke(this);
        _canvas = _canvasPC; //TODO -> check platform
        EntityHUD.OnEntitySelected += SelectEntity;
        HUD.OnSkillSelected += SelectSkill;
        MissionManager.OnNextWave += InitBattle;
        MissionManager.OnMissionComplete += EndBattle;
        InitBattle();
    }

    private void OnDestroy()
    {
        EntityHUD.OnEntitySelected -= SelectEntity;
        HUD.OnSkillSelected -= SelectSkill;
        MissionManager.OnNextWave -= InitBattle;
        MissionManager.OnMissionComplete -= EndBattle;
    }

    public void InitBattle()
    {
        //TODO -> set background
        _background.sprite = MissionManager.Instance.CurrentMission.MissionBackground != null ? MissionManager.Instance.CurrentMission.MissionBackground : Resources.Load<Sprite>( $"Textures/Backgrounds/V1_PRAIRIE");
        //TODO -> show turn n° ?
        Targets = new();

        LostPopUp.SetActive(false);
        WonPopUp.SetActive(false);

        PlayerHasWin = false;
        Turn = 1;

        if (MissionManager.Instance.CurrentMission == null)
        {
            Debug.Log("Start Simulation");
            OnSimulationStart?.Invoke(this);
            return;
        }

        if (!MissionManager.Instance.IsFirstWaveCleared)
        {
            Debug.Log($"Starting mission : {MissionManager.Instance.CurrentMission.Name}");
        }
        else
        {
            Debug.Log($"Starting next wave : {MissionManager.Instance.CurrentMission.Name}");
        }

        InitPlayer();
        InitSupports();
        InitEnemies();

        SetState(new WhoGoFirst(this));
    }

    private void InitPlayer(Player player = null)
    {
        if (!MissionManager.Instance.IsFirstWaveCleared)
        {
            Debug.Log("Creating new battle datas for player");
            Player = player ?? new();
            var playerGO = Instantiate(_entitySlot, _canvas.transform);
            playerGO.tag = "Player";
            Destroy(playerGO.GetComponent<BoxCollider2D>());
            Player.HUD = playerGO.GetComponent<EntityHUD>();
            Player.Name = "Player";
            Player.HUD.Init((Player)Player);
        }

        Player.InitElement();

        int position = 0;

        foreach (var skill in Player.Skills)
        {
            //TODO -> Set position
            skill.ConstantPassive(Enemies, Player, Turn, Allies); // constant passive at battle start
            //skill.Button = Instantiate(_skillButton, _canvas.transform).GetComponent<SkillHUD>();
            //skill.Button.Init(skill, position);
            
            skill.SkillButton = Instantiate(_skillSlot, _canvas.transform).GetComponent<SkillButton>();
            skill.SkillButton.Init(skill, position);
            
            position += 160; //TODO -> dynamic position
        }
    }

    private void InitSupports()
    {
        int position = 100;

        foreach (var support in Player.EquippedSupports)
        {
            SupportHUD hud = Instantiate(_supportSlot, _canvas.transform).GetComponent<SupportHUD>();

            support?.Init();
            hud.Init(support, position);
            position -= 190; //TODO -> dynamic position

            if (support == null) continue;
            support.Button = hud;

            foreach (var skill in support.Skills)
            {
                skill.ConstantPassive(Enemies, Player, Turn, Allies); // constant passive at battle start
            }
        }
    }

    private void InitEnemies()
    {
        //TODO -> show enemies id
        MissionSO mission = MissionManager.Instance.CurrentMission;
        int wave = MissionManager.Instance.CurrentWave;

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
            Enemy enemy = new(id, Resources.Load<EnemySO>($"SO/Enemies/{enemyName}"));
            Enemies.Add(enemy);
            id++;
        }

        InstantiateEnemies();
    }

    private void InstantiateEnemies()
    {
        foreach (var enemy in Enemies)
        {
            enemy.HUD = Instantiate(_entitySlot, _canvas.transform).GetComponent<EntityHUD>();
            enemy.HUD.Init(enemy, enemy.Id);

            //int x = enemy.Id == 1 ? 480 : enemy.Id == 2 ? 45 : enemy.Id % 2 == 0 ? 480 : 45;
            int x = enemy.Id < 3 ? enemy.Id % 2 == 0 ? 45 : 480 : enemy.Id % 2 == 0 ? 480 : 45;
            int y = enemy.Id > 4 ? -250 : enemy.Id > 2 ? 250 : 0;

            if (enemy.Id > 2) x += 250;

            enemy.HUD.transform.localPosition = new Vector3(x, y, 0);
        }

        InitAttackBar();
    }

    private void InitAttackBar()
    {
        AttackBarSystem = new(Player, Enemies);
        AttackBarSystem.InitAtkBars();
        //UpdateEntities();
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
            //if (Player.HasEffect(new Silence())) break;
        }

        //if (Player.HasEffect(new SupportSilence())) return;

        foreach (var support in Player.EquippedSupports)
        {
            if (support == null) continue;
            support.Button.ToggleHUD(active);
        }
    }

    private void SelectEntity(Entity entity)
    {
        //Debug.Log("Entity seleted.");
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
            Debug.Log($"Killed {target.Name}");
            Destroy(target.HUD.gameObject);
            OnEnemyKilled?.Invoke(target.Name);
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
            skill.PassiveBeforeAttack(Enemies, Player, Turn, Allies);
        }

        totalDamage += selectedSkill.SkillBeforeUse(Targets, Player, Turn, Allies);
        totalDamage += selectedSkill.Use(Targets, Player, Turn, Allies);
        totalDamage += selectedSkill.AdditionalDamage(Targets, Player, Turn, totalDamage, Allies);

        selectedSkill.SkillAfterDamage(Targets, Player, Turn, totalDamage, Allies);

        foreach (var skill in Player.Skills)
        {
            skill.PassiveAfterAttack(Enemies, Player, Turn, totalDamage, Allies);
        }
    }

    public void UpdateEntities()
    {
        for (int i = 0; i < AttackBarSystem.AllEntities.Count - 1; i++)
        {
            Enemies[i] = AttackBarSystem.AllEntities[i];
        }

        Player = AttackBarSystem.AllEntities[AttackBarSystem.AllEntities.Count - 1];
        foreach (var entity in Enemies)
        {
            entity.ResetHealed();
        }
        Player.ResetHealed();
    }

    public void EndWave(bool won)
    {
        //TODO -> add end wave animation ?
        //DialogueText.text = won ? "YOU WON" : "YOU LOST";

        foreach (var skill in Player.Skills) Destroy(skill.Button);

        foreach (var support in Player.EquippedSupports)
        {
            if (support != null) Destroy(support.Button);
        }

        //foreach (var skill in Player.Skills) skill.TakeOffStats(Enemies, Player, 0); //constant passives at battle end
        foreach (var stat in Player.Stats) stat.Value.RemoveAllModifiers();
        if (won)
        {
            OnWaveCompleted?.Invoke();
            return;
        }

        OnPlayerLose?.Invoke(true);
        //TODO -> Load game over popup
    }

    private void EndBattle(MissionSO mission)
    {
        //TODO -> Load end mission popup
        Debug.Log("Battle ended");
        OnBattleCompleted?.Invoke();
    }

    #region AutoBattle
    public void SimulateBattle(Player player, List<Entity> enemies)
    {
        Enemies = enemies;

        InitPlayer(player);
        InitSupports();
        InstantiateEnemies();
        //SetState(new AutoBattle(this));
        SetState(new WhoGoFirst(this));
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