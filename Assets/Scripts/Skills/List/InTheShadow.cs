using System.Collections.Generic;

public class InTheShadow : Skill
{
    private void Awake()
    {
        FileName = "InTheShadow";
    }

    public override void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {
        player.DefIgnored = 0.4f;
    }

     // to do : if enemy is debuff, #% chance to play again
}