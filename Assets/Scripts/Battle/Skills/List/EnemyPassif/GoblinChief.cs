using System.Collections.Generic;

public class GoblinChief : PassiveSkill
{
    public override void PassiveBeforeAttack(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        foreach (Entity target in targets)
        {
            foreach (Effect effect in target.Effects)
            {
                if (effect.GetType() == typeof(Stun))
                {
                    target.ApplyEffect(new BreakAttack());
                }
            }
        }
    }
}