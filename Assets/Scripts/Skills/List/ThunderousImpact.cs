using System.Collections.Generic;

public class ThunderousImpact : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        //TODO -> stun for 1 turn
        return 0;
    }
}