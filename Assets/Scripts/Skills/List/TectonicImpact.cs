using System.Collections.Generic;

public class TectonicImpact : Skill
{
//TODO -> When the player attacks a single enemy, deal 15% of damage done to all other enemies.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }

    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }

    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }

    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }

    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }

    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn) { }
}