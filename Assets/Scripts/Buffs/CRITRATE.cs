
    using System.Collections.Generic;

    public class CRITRATE : Effect
    {
        private float _percentage => 1.5f;

        public CRITRATE(int duration, Entity target)
        {
            ModifiedStats = new List<string>
            {
                "CritRate",
            };
            Duration = duration;
            target.CritRate *= _percentage;
        }
    }
