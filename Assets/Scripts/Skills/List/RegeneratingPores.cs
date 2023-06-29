using System.Collections.Generic;

public class RegeneratingPores : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        for (int i = 1; i < targets.Count; i++)
        {
            targets[i].CurrentHp += (targets[i].Stats[Item.AttributeStat.HP].Value * 0.6f);
        }

        return 0;
    }

    // add the clense all debuffs
}