using System.Collections.Generic;

public class DEFBUFF : Effect
{
    private float _percentage => 1.5f;

    public DEFBUFF(int duration, Entity target)
    {
        ModifiedStats = new List<string> { "PhysDef", "MagicDef" };
        Duration = duration;
        target.PhysDef *= _percentage;
        target.MagicDef *= _percentage;
    }
}