using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ScriptableObject enemyModifier;

    private Slider _hpBar;
    public Enemy _enemy { get; private set; }

    private void Awake()
    {
        _enemy = new Enemy();
    }

    private void Start()
    {
        InitHUD();
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void OnMouseDown()
    {
        _enemy.HaveBeSelected();
        Debug.Log("Enemy is selected" + _enemy.IsSelected);
    }

    private void InitHUD()
    {
        _hpBar = GetComponentInChildren<Slider>();
        _hpBar.maxValue = _enemy.MaxHp;
        _hpBar.value = _enemy.CurrentHp;
    }

    public void InitEnemy()
    {


    }

    private void UpdateHUD()
    {
        _hpBar.value = _enemy.CurrentHp;
    }
}