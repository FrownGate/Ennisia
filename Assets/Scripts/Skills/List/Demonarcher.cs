using System.Collections.Generic;

public class Demonarcher : Skill
{
//TODO -> Increase all defense by 20%
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }

    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }

    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }

    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }

    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }

    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }

    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
}
