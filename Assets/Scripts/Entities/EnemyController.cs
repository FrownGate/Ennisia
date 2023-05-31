using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ScriptableObject enemyModifier;

    private Slider _hpBar;
    public Enemy Enemy { get; private set; }

    private void Awake()
    {
        Enemy = new Enemy();
    }

    private void Start()
    {
        InitHUD();
    }

    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Enemy.HaveBeSelected();
        Debug.Log("Enemy is selected" + Enemy.IsSelected);
    }

    private void InitHUD()
    {
        _hpBar = GetComponentInChildren<Slider>();
        _hpBar.maxValue = Enemy.MaxHp;
        _hpBar.value = Enemy.CurrentHp;
    }

    public void InitEnemy()
    {
        //
    }

    public void UpdateHUD()
    {
        _hpBar.value = Enemy.CurrentHp;
    }

    public void ResetStats()
    {
        Enemy = new Enemy();
    }
}