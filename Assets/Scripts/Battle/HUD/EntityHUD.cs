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

    private Entity _entity;
    private int _id;

    private void Update()
    {
        if (_entity == null) return;
        _hpBar.value = _entity.CurrentHp >= 0 ? _entity.CurrentHp : 0;
        _text.text = Mathf.Round(_entity.CurrentHp) + "/" + Mathf.Round(_entity.Stats[Attribute.HP].Value);
    }

    public void Init(Entity entity, int id = 0)
    {

        _sprite.sprite = entity.EntitySprite;
        if (_sprite.sprite == null)
        {
            _sprite.sprite = Resources.Load<Sprite>($"Textures/Enemies/Player");
        }
        //TODO -> Show Id on HUD
        //Tempo parce que c'est uwu
        switch (entity.Name)
        {
            case "Wolf Pack Leader":
            case "Player":
                _sprite.flipX = true;
                break;
        }

        ////////
        Debug.Log($"Assets/Resources/Textures/Enemies/{entity.Name}");
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

        gameObject.name = entity.Name;
    }

    private void OnMouseUpAsButton()
    {
        OnEntitySelected?.Invoke(_entity);
    }
}