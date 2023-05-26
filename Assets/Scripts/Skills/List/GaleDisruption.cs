using System.Collections.Generic;

public class GaleDisruption : Skill
{
    private void Awake()
    {
        FileName = "GaleDisruption";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        //TODO -> cleanse buffs of target
        return 0;
    }
}