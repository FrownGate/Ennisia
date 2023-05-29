using System.Collections.Generic;

public class BreezeOfVitality : Skill
{
    private readonly float _increaseHealPerc = 10f;

    private void Awake()
    {
        FileName = "BreezeOfVitality";
    }

    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float addHeal = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].CurrentHp > 0)
            {
                addHeal += _increaseHealPerc;
            }
        }

        float heal = player.MaxHp * (Data.HealingAmount + addHeal) / 100;
        player.CurrentHp += heal;

        return 0;
    }
}