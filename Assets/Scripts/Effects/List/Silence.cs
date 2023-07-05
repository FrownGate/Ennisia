public class Silence : Effect
{
    public Silence(int? duration = null) : base(duration) { }

    public override void AlterationEffect(Entity target)
    {
        if (target is Player player)
        {
            player.Weapon.Skills[0].SkillButton.interactable = false;
            player.Weapon.Skills[1].SkillButton.interactable = false;
        }
        else if (target is Enemy enemy)
        {
            enemy.Skills[1].SkillButton.interactable = false;
            enemy.Skills[2].SkillButton.interactable = false;
        }
    }
}