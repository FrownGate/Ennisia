using System.Collections.Generic;

public class AspectoftheFallenAngel : DamageSkill
{
//TODO -> Deal damage equal to 60% of his defense and inflicts defense break (debuff) to caster for 2 turns. Cooldown 3 turns. If battle difficulty is insane, deal damage equal to 60% of his attack and inflicts an Attack break (debuff) to caster for 2 turns. Cooldown 3 turns.
    public override float Use(List<Entity> targets, Entity caster, int turn)
    {
        if(MissionManager.Instance.CurrentMission.NumInChapter < 6)
        {
            foreach (Entity target in targets)
            {
                float damage = DamageCalculation(target, caster, Attribute.PhysicalDefense, Attribute.MagicalDefense);
                target.TakeDamage(damage * Data.DamageRatio);
                TotalDamage += damage;
            }
            return TotalDamage;
        }
        else
        {
            foreach (Entity target in targets)
            {
                float damage = DamageCalculation(target, caster, Attribute.Attack);
                target.TakeDamage(damage * Data.DamageRatio);
                TotalDamage += damage;
            }

            return TotalDamage;
        }
    }
    public override void PassiveAfterAttack(List<Entity> targets, Entity caster, int turn, float damage)
    {
        if (MissionManager.Instance.CurrentMission.NumInChapter < 6)
        {
            caster.ApplyEffect(new BreakDefense(2));
        }
        else
        {
            caster.ApplyEffect(new BreakAttack(2));
        }
    }
}
