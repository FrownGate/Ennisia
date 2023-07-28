using System.Collections.Generic;

public class LevelAdjustment : Skill
{
//TODO -> Before attacking, if the user(Betala) has a debuff, and the player has a buff, inflict a support silence(debuff) to the player and give immunity to the user(Betala).
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn, List<Entity> allies) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn, List<Entity> allies) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage, List<Entity> allies) { }
}
