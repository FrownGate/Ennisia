using System;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class EnemyController : Controller
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private ScriptableObject enemyModifier;
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
            hpBar.maxValue = _enemy.MaxHp;
            hpBar.value = _enemy.CurrentHp;
        }

        private void UpdateHUD()
        {
            hpBar.value = _enemy.CurrentHp;
        }
    }
}