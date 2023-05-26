using System.Collections.Generic;

public class FlamePurification : Skill
{
    private void Awake()
    {
        FileName = "FlamePurification";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //TODO -> cleanse all debuff
        return 0;
    }
}