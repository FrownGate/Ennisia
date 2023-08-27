using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EntityHUD : MonoBehaviour
{
    public static event Action<Entity> OnEntitySelected;
    
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TextMeshProUGUI _idText;
    [SerializeField] private EffectHUD _effectHUD;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private BattleAnimation _battleAnimation;
    [SerializeField] private TMP_Text _damagesPrefab;
    [SerializeField] private AnimationClip _damageAnimation;
  
    private Entity _entity;
    private int _id;
    private float _previousHp;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        Entity.OnDamageTaken += ShowDamages;
    }

    private void OnDestroy()
    {
        Entity.OnDamageTaken -= ShowDamages;
    }

    private void Update()
    {
        if (_entity == null) return;

        _hpText.text = Mathf.Round(_entity.CurrentHp) + "/" + Mathf.Round(_entity.Stats[Attribute.HP].Value);
        
        float currentFillAmount = _entity.CurrentHp / _entity.Stats[Attribute.HP].Value;
        
        if(currentFillAmount != _previousHp)
        {
            _healthBar.hpImg.fillAmount = currentFillAmount;
            StartCoroutine(_healthBar.UpdateHpEffectCoroutine());
            _battleAnimation.DamageTaken();
        }
        
        _previousHp = currentFillAmount;
    }

    public void Init(Entity entity, int id = 0)
    {

        _sprite.sprite = entity.EntitySprite;

        if (_sprite.sprite == null)
        {
            _sprite.sprite = Resources.Load<Sprite>($"Textures/Enemies/Player");
        }
        
        Debug.Log($"Assets/Resources/Textures/Enemies/{entity.Name}");
        _entity = entity;
        _effectHUD.Init(_entity);
        _id = id;
        _idText.text = "ID:" + _id;

        if (entity is Player)
        {
            _sprite.flipX = true;
        }
        
        if (entity is not Player && !entity.IsBoss) //TODO -> check if boss
        {
            transform.localScale = transform.localScale / 1.5f;
            return;
        }

        transform.localPosition = new Vector3(-495, 0, 0);

        gameObject.name = entity.Name;
    }

    private void ShowDamages(Entity entity, int damage, Color color)
    {
        if (_entity == null || entity != _entity) return;
        var damages = Instantiate(_damagesPrefab, _canvas.transform);
        damages.transform.localPosition = Vector3.zero;
        damages.text = $"-{damage}";
        damages.color = color;
        StartCoroutine(Wait(damages));
    }

    private IEnumerator Wait(TMP_Text damages)
    {
        yield return new WaitForSeconds(_damageAnimation.length);
        Destroy(damages.gameObject);
    }

    private void OnMouseUpAsButton()
    {
        OnEntitySelected?.Invoke(_entity);
    }
    
    //TODO: Temporary
    public void AttackAnimation()
    {
        _battleAnimation.MadeAttack();
    }
}