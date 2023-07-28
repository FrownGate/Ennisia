using System.Collections.Generic;

public class ShieldBash : Skill
{
//TODO -> Inflicts damage based on 25% of his defense and has a 40% chance of Taunting. 
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
}
