using System.Collections.Generic;

public class Reinforcement : Skill
{
    private void Awake()
    {
        FileName = "Reinforcement";
    }

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float PhdefBuff = player.PhysDef * 0.05f;
        float MdefBuff = player.MagicDef * 0.05f;
        player.Attack = PhdefBuff + MdefBuff;
    }
}