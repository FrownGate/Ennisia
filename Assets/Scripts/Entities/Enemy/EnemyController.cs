using System;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class EnemyController : Controller
    {
        [SerializeField] private ScriptableObject enemyModifier;
        
        private Slider _hpBar;
        public Enemy _enemy { get; private set; }

        private void Start()
        {
            _enemy = new Enemy();
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
            _enemy.TakeDamage(10);
        }

        private void InitHUD()
        {
            _hpBar = GetComponentInChildren<Slider>();
            _hpBar.maxValue = _enemy.MaxHp;
            _hpBar.value = _enemy.CurrentHp;
        }

        private void UpdateHUD()
        {
            _hpBar.value = _enemy.CurrentHp;
        }
    }
}