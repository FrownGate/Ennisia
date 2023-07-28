using System.Collections.Generic;

public class HerbsMaster : ProtectionSkill
{
    private float _lowestEnemy;
    private int _enemyNum;

    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        int lowestEnemyIndex = allies.Count > 2 ? FindLowestAlly(allies) : 1;
        allies[lowestEnemyIndex].Heal(allies[lowestEnemyIndex].Stats[Attribute.HP].Value * Data.HealingAmount);
        return 0;
    }

    private int FindLowestAlly(List<Entity> allies)
    {
        _lowestEnemy = allies[1].Stats[Attribute.HP].Value;
        for (int i = 2; i < allies.Count; i++)
        {
            if (_lowestEnemy >= allies[i].Stats[Attribute.HP].Value)
            {
                _enemyNum = i;
            }
        }
        return _enemyNum;
    }
}