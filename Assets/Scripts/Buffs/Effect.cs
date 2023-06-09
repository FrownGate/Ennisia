    using System.Collections.Generic;
    
    public abstract class Effect
    {
        public string Description { get; set; }
        public string Name { get; set; }
        protected internal List<string> ModifiedStats { get; protected set; }
        protected internal int Duration { get; protected set; }
        
        public void Tick()
        {
            Duration --;
        }

        public void ResetDuration(int duration)
        {
            Duration = duration;
        }
        
    }
