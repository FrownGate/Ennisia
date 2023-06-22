using System.Collections.Generic;

public class ThunderousImpact : DamageSkill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //TODO -> stun for 1 turn
        return 0;
    }
}