
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class EffectHUD : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _effectIconPrefab;
        [SerializeField] private Sprite[] _sprites;

        private Entity _entity { get; set; }
        private List<GameObject> _effectIcons = new();
        public List<String> Effects { get; set; } = new();
        
        private void Update()
        {
            if (_entity.Effects.Count != _effectIcons.Count)
            {
                foreach (var effect in _entity.Effects)
                {
                    if (Effects.Contains(effect.Data.Name) && !_effectIcons.Exists(x => x.name == effect.Data.Name))
                    {
                        AddEffectIcon(effect.Data.Name);
                    }
                }
            }
        }
        
        public void Init(Entity entity)
        {
            _entity = entity;
            InitExistingEffect();
        }

        private void AddEffectIcon(string effectName)
        {
            var effect = Instantiate(_effectIconPrefab, _container.transform);
            effect.transform.SetParent(_container.transform);
            effect.GetComponent<SpriteRenderer>().sprite = _sprites[Effects.IndexOf(effectName)];
            _effectIcons.Add(effect);
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
