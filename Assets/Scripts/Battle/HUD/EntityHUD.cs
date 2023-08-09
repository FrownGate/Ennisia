using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityHUD : MonoBehaviour
{
    public static event Action<Entity> OnEntitySelected;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private EffectHUD EffectHUD;

    private Entity _entity;
    private int _id;

    private void Start()
    {
        EffectHUD.Init(_entity);
    }

    private void Update()
    {
        if (_entity == null) return;
        _hpBar.value = _entity.CurrentHp >= 0 ? _entity.CurrentHp : 0;
        _text.text = Mathf.Round(_entity.CurrentHp) + "/" + Mathf.Round(_entity.Stats[Attribute.HP].Value);
    }

    public void Init(Entity entity, int id = 0)
    {

        _sprite.sprite = Resources.Load<Sprite>(entity.Name != "" ? $"Textures/Enemies/{entity.Name}" : $"Textures/Enemies/Player");
        //Tempo parce que c'est uwu
        if (entity.Name == "Wolf Pack Leader") _sprite.flipX = true;
        ////////
        Debug.Log($"Assets/Resources/Textures/Enemies/{entity.Name}");
        //TODO -> Show Id on HUD
        _entity = entity;
        _id = id;
        _hpBar.maxValue = _entity.Stats[Attribute.HP].Value;
        _hpBar.value = _entity.CurrentHp;

        if (entity is not Player && !entity.IsBoss) //TODO -> check if boss
        {
            transform.localScale = transform.localScale / 1.5f;
            return;
        }

        transform.localPosition = new Vector3(-495, 0, 0);
        
    }

    private void OnMouseUpAsButton()
    {
        OnEntitySelected?.Invoke(_entity);
    }
}