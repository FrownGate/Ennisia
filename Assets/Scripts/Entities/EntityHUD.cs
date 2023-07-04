using System;
using UnityEngine;
using UnityEngine.UI;

public class EntityHUD : MonoBehaviour
{
    public static event Action<Entity> OnEntitySelected;

    [SerializeField] private Slider _hpBar;
    [SerializeField] private SpriteRenderer _sprite;

    private Entity _entity;

    private void Update()
    {
        if (_entity == null) return;
        _hpBar.value = _entity.CurrentHp >= 0 ? _entity.CurrentHp : 0;
    }

    public void Init(Entity entity)
    {
        //TODO -> set sprite
        _entity = entity;

        _hpBar.maxValue = _entity.Stats[Attribute.HP].Value;
        _hpBar.value = _entity.CurrentHp;

        if (entity is not Player) return;
        transform.localPosition = new Vector3(-495, 0, 0);
    }

    private void OnMouseUpAsButton()
    {
        OnEntitySelected?.Invoke(_entity);
    }
}