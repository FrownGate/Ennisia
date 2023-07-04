using System.Collections.Generic;

public class Thief : DamageSkill
{
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        return 0;
    }

    // add atk debuff to player and atk buff to enemy
}