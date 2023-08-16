
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Image = UnityEngine.UI.Image;
    using Random = UnityEngine.Random;

    public class EffectHUD : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _effectIconPrefab;
        [SerializeField] private Sprite[] _sprites;

        private Entity _entity { get; set; }
        private List<GameObject> _effectIcons = new();
        private Image _effectIcon;
        public List<String> Effects { get; set; } = new();
        private HashSet<string> _processedEffects = new HashSet<string>();

        private void Start()
        {
            InitExistingEffect();
        }

        private void Update()
        {
            foreach (var effect in _entity.Effects)
            {
                if (effect.IsExpired)
                {
                    RemoveEffectIcon(effect.Data.Name);
                    _processedEffects.Remove(effect.Data.Name);
                }
                // If it's a new effect we haven't processed yet
                if (Effects.Contains(effect.Data.Name) && !_processedEffects.Contains(effect.Data.Name))
                {
                    AddEffectIcon(effect.Data.Name);
                    _processedEffects.Add(effect.Data.Name);
                }
            }
        }
        
        public void Init(Entity entity)
        {
            _entity = entity;
          
        }

        private void AddEffectIcon(string effectName)
        {
            var effect = Instantiate(_effectIconPrefab, _container.transform);
            effect.transform.SetParent(_container.transform);
            effect.SetActive(true);
            effect.name = effectName;
            _effectIcon = effect.GetComponent<Image>();
            _effectIcon.sprite = _sprites[Random.Range(0,3)];
            Debug.LogWarning("Created Effect Icon");
            _effectIcons.Add(effect);
        }
        
        public void RemoveEffectIcon(string effectName)
        {
            var effect = _effectIcons.Find(x => x.name == effectName);
            _effectIcons.Remove(effect);
            Destroy(effect);
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
