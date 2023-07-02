using System.Collections.Generic;

public class HerbsMaster : ProtectionSkill
{
    private float _lowestEnemy;
    private int _enemyNum;

    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        int lowestEnemyIndex = targets.Count > 2 ? FindLowestAlly(targets) : 1;
        targets[lowestEnemyIndex].CurrentHp += (targets[lowestEnemyIndex].Stats[Attribute.HP].Value * 0.5f);

        return 0;
    }

    private int FindLowestAlly(List<Entity> targets)
    {
        _lowestEnemy = targets[1].Stats[Attribute.HP].Value;
        for (int i = 2; i < targets.Count; i++)
        {
            if (_lowestEnemy >= targets[i].Stats[Attribute.HP].Value)
            {
                _enemyNum = i;
            }
        }
        return _enemyNum;
    }
}