using UnityEngine;

public class EnemyController : EntityController
{
    [SerializeField] private ScriptableObject _enemyModifier;

    public override void InitEntity()
    {
        Entity = new Enemy();
    }

    private void OnMouseDown()
    {
        Entity.HaveBeenSelected();
        Debug.Log("Enemy is selected" + Entity.IsSelected);
    }
}