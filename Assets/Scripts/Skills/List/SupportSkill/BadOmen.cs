using System.Collections.Generic;

public class BadOmen : BuffSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        caster.ApplyEffect(new DefenseBuff());
        caster.Shield += (int)(caster.Stats[Attribute.HP].Value * 0.25f);
        return 0;
    }
}