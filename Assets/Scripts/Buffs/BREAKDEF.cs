
    using System.Collections.Generic;

    public class BREAKDEF : Effect
    {
        private float _percentage => 0.7f;
        public BREAKDEF(int duration, Entity target)
        {
            ModifiedStats = new List<string>
            {
                "Attack",
            };
            Duration = duration;
            target.PhysDef  *= _percentage;
            target.MagicDef *= _percentage;
        }
    }
