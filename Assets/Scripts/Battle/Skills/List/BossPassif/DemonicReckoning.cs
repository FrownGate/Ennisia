using System.Collections.Generic;

public class DemonicReckoning : Skill
{
//TODO -> If the player has 6 or more Demonic Marks, Consumes all Demonic Marks on the player and inflicts a defense break (debuff) before dealing 50% of attack as physical damage. Has a 50% (100% on insane) chance to attack again for each Demonic Marks, each hit deals damage equivalent to 25% of the boss's Attack as Magical Damages. After attacking, inflict an attack (debuff).
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
}
