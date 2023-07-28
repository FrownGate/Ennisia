using System.Collections.Generic;

public class AspectoftheFallenAngel : Skill
{
//TODO -> Deal damage equal to 60% of his defense and inflicts defense break (debuff) to caster for 2 turns. Cooldown 3 turns. If battle difficulty is insane, deal damage equal to 60% of his defense and inflicts an Attack break (debuff) to caster for 2 turns.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
}
