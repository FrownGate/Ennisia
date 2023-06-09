using System.Collections.Generic;

public class AquaParalysis : Skill
{
//TODO -> When the player is attacked, has a 15% chance to stun the enemy.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }

    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }

    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }

    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }

    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }

    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }

    public override void TakeOffStats(List<Entity> targets, Entity player, int turn) { }
}
