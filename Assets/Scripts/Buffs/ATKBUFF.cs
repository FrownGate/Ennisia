

    using System.Collections.Generic;

    public class ATKBUFF : Effect
    {
        private float _percent;
        public ATKBUFF(int duration,Entity target)
        {
            ModifiedStats = new List<string>{"Attack"};
            _percent = 1.5f;
            Duration = duration;
            target.Attack *= _percent;
        }
    }
