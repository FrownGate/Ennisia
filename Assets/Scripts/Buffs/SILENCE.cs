using Random = System.Random;

public class SILENCE : Effect
{
    public SILENCE(int duration, Entity target)
    {
        Duration = duration;
        IsSilenced(target.Skills[new Random().Next(1, 3)], Duration);
    }

    private void IsSilenced(Skill skill, int duration)
    {
        skill.ResetCoolDown(duration);
    }
}