using System.Collections.Generic;

public class NurturingEarthbound : Skill
{
    private void Awake()
    {
        FileName = "NurturingEarthbound";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float lostHealt = player.MaxHp - player.CurrentHp;
        player.CurrentHp += lostHealt * Data.healingAmount / 100;
        return 0;
    }
}