using System.Collections.Generic;

public class MalevolentStrike : PassiveSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn)
    {
        foreach (Effect effect in caster.Effects)
        {
            if (effect.GetType() == typeof(AttackBuff))
            {
                foreach(Entity target in targets)
                {
                    target.DefIgnored += 10;
                }
            }
        }
    }

    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        foreach (Entity target in targets)
        {
            target.DefIgnored -= 10;
        }
    }
}