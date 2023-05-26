using System.Collections.Generic;

public class HawksEye : Skill
{
    private void Awake()
    {
        FileName = "HawksEye";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //give CR / CD / ATTACK Buff
        //give additional turn
        return 0;
    }
}