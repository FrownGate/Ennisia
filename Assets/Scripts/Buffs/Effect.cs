
    public abstract class Effect
    {
        public string Description { get; set; }
        public string Name { get; set; }

        public enum EffectType
        {
            Buff = 1,
            Debuff = 2, 
        }
        
        protected internal EffectType _effectType { get; protected set; }
        private int Duration { get; set; }
        
        public void Tick()
        {
            Duration --;
        }

        public void GetBaseValue()
        {
            
        }
        
    }
