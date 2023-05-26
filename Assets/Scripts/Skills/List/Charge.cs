using System.Collections.Generic;

public class Charge : Skill
{
    private void Awake()
    {
        FileName = "Charge";
    }

    public override void ConstantPassive(List<Entity> target, Entity player, int turn)
    {
        float attackBuff = player.Attack * 0.05f;
        player.Attack = attackBuff;
    }
}