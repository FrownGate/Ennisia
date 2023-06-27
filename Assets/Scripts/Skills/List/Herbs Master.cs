using System.Collections.Generic;

public class HerbsMaster : ProtectionSkill
{
    private float lowestEnemy;
    private int enemyNum;

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        int lowestEnemyIndex = targets.Count > 2 ? FindLowestAlly(targets) : 1;
        targets[lowestEnemyIndex].CurrentHp += (targets[lowestEnemyIndex].Stats[Item.AttributeStat.HP].Value * 0.5f);

        return 0;
    }

    private int FindLowestAlly(List<Entity> targets)
    {
        lowestEnemy = targets[1].Stats[Item.AttributeStat.HP].Value;
        for (int i = 2; i < targets.Count; i++)
        {
            if (lowestEnemy >= targets[i].Stats[Item.AttributeStat.HP].Value)
            {
                enemyNum = i;
            }
        }
        return enemyNum;
    }
}
