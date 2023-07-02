using System.Collections.Generic;

public class WishingStar : ProtectionSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        for (int i = 1; i < targets.Count; i++)
        {
            targets[i].CurrentHp += (targets[i].Stats[Attribute.HP].Value * 0.2f);
            targets[i].Shield += (int)(targets[i].Stats[Attribute.HP].Value * 0.15f);
        }

        return 0;
    }
}