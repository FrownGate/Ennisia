using System.Collections.Generic;

public class NobleGratitude : Skill
{
//TODO -> Everytime the user(Betala) receives a buff, adds one buff mark. Once the number of buff marks reaches 5, If the user has a buff, deals magic damage equivalent to 70% of attack for each buff active.
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn) { }
    public override void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn) { }
    public override float SkillBeforeUse(List<Entity> targets, Entity player, int turn) { return 0; }
    public override float Use(List<Entity> targets, Entity player, int turn) { return 0; }
    public override float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage) { return 0; }
    public override void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage) { }
    public override void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage) { }
}
