using System;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Entities
{
    public class PlayerController : Controller
    {
        public Player _player { get; private set; }
        private Slider _hpBar;
        private void Start()
        {
            _player = new Player();
            InitHUD();
        }

        private void Update()
        {
            UpdateHUD();
        }

        private void InitHUD()
        {
            _hpBar = GetComponentInChildren<Slider>();
            _hpBar.maxValue = _player.MaxHp;
            _hpBar.value = _player.CurrentHp;
        }

        private void UpdateHUD()
        {
            _hpBar.value = _player.CurrentHp;
        }
    }
}
