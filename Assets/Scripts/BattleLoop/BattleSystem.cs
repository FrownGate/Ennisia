using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class BattleSystem : StateMachine
{
    [SerializeField] private GameObject Support1;
    [SerializeField] private GameObject Support2;

    [SerializeField] public GameObject[] SkillsButton;

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

    private void Start()
    {
        Enemies = new List<Entity>();
        Targetables = new List<Entity>();
        //Entity
        EnemyContainer();
        InitPlayer();

        foreach (var skill in Allies[0].Skills)
        {
            skill.ConstantPassive(Enemies, Allies[0], 0); // constant passive at battle start
        }

        SetState(new WhoGoFirst(this));
    }

    private void EnemyContainer()
    {
        GameObject enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");
        Enemies.Add(enemyPrefab.GetComponent<EnemyController>().Enemy);
        Vector3 gridCenter = EnemyStation.position;
        int numRows = 1;
        int numColumns = 1;
        float hexagonSize = 1f;
        float hexagonSpacing = 5f;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                float xPos = col * (hexagonSize + hexagonSpacing);
                float yPos = row * (hexagonSize + hexagonSpacing) * Mathf.Sqrt(3);
                if (row % 2 == 1)
                    xPos += (hexagonSize + hexagonSpacing) / 2f;

                Vector3 hexagonPosition = gridCenter + new Vector3(xPos + 10, yPos, 0f);

                GameObject enemyInstance = Instantiate(enemyPrefab, hexagonPosition, Quaternion.identity);
                EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
                Enemy tmp = enemyController.Enemy;
                Enemies.Add(tmp);
            }
        }
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

    public void OnMouseUp()
    {
        _selected = true;
        if (_selected)
        {
            Targetables = GetSelectedEnemies(Enemies);
            foreach (var enemies in Targetables)
            {
                Debug.Log(enemies);
            }

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

    public Skill GetSelectedSkill(int buttonId)
    {
        if (buttonId < 0 || buttonId >= Allies[0].Skills.Count)
        {
            Debug.LogError("Invalid button ID");
            return null;
        }

        return Allies[0].Skills[buttonId];
    }
}