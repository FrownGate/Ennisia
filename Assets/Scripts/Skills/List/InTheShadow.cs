using System.Collections.Generic;

public class InTheShadow : Skill
{
    public float DefIgnoredBaseRatio;
    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        float DefIgnoredBuff = DefIgnoredBaseRatio + (StatUpgrade1 * Level);
        player.DefIgnored = DefIgnoredBuff;
    }

     // to do : if enemy is debuff, #% chance to play again
}