
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class EffectHUD : HUD
    {
        [SerializeField] private Sprite[] _sprites;

        
        public Sprite[] Sprites
        {
            get => _sprites; 
            set => _sprites = value;
        }

        private Entity _entity { get; set; }
        public List<String> Effects { get; set; } = new();

        private void Start()
        {
            InitExistingEffect();
            
        }

        private void InitExistingEffect()
        {
            Effects = new List<string>
            {
                "Taunt",
                "Attack Buff",
                "Berserk",
                "Break Attack",
                "Break Defense",
                "CritDmg Buff",
                "CritRate Buff",
                "Defense Buff",
                "Demonic Mark",
                "Immunity",
                "Silence",
                "Stun",
                "Support Silence",
            };
        }
        
        
    }
