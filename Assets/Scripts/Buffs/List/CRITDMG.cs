using System.Collections.Generic;

public class CRITDMG : BuffEffect
{
    private float _percentage => 1.5f;

    public CRITDMG(int duration, Entity target)
    {
        ModifiedStats = new List<string> { "CritDamage" };
        Duration = duration;
        target.CritDamage *= _percentage;
    }
}