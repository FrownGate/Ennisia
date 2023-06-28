using System.Collections.Generic;

public class WishingStar : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        for (int i = 1; i < targets.Count; i++)
        {
            targets[i].CurrentHp += (targets[i].Stats[Item.AttributeStat.HP].Value * 0.2f);
            targets[i].Shield += (int)(targets[i].Stats[Item.AttributeStat.HP].Value * 0.15f);
        }

        return 0;
    }
}