using System.Collections.Generic;

public class DemonicGamble : Skill
{
//TODO -> Attacks the player with a 50% chance to stun dealing 70% of attack as physical damage, if the player is stunned after attack, inflict 2 Demonic Mark. If the player is not stunned after the attack, inflict a defense break (debuff) to the user.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
}
