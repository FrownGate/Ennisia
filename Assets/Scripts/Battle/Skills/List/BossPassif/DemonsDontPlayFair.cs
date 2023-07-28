using System.Collections.Generic;

public class DemonsDontPlayFair : Skill
{
//TODO -> If the user has a debuff before attacking, strip the player of all buffs.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
}
