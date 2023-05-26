using System.Collections.Generic;

public class ThunderstrikeAssassination : Skill
{
    private void Awake()
    {
        FileName = "ThunderstrikeAssassination";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        return 0;
    }
}