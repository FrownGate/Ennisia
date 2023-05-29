using System.Collections.Generic;

public class TorrentialAnnihilation : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (player.Attack > targets[i].Attack)
            {
                //TODO -> cleanse target's buff
            }
        }

        return 0;
    }

    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].TakeDamage(damage * 0.25f);
        }

        return 0;
    }
}