using System.Collections.Generic;

public class AspectoftheGuardianAngel : BuffSkill
{
//TODO -> Gain Immunity for 2 turns and Silence the Supports Characters of the enemy for 2 turns. Cooldown 4 turns.
    public override float Use(List<Entity> targets, Entity caster, int turn, List<Entity> allies)
    {
        caster.ApplyEffect(new Immunity());
        foreach (Entity target in targets)
        {
            target.ApplyEffect(new SupportSilence());
        }
        return 0;
    }
}
