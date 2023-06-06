
    public abstract class Buff
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private int Duration { get; set; }
        
        
        
        public void Tick()
        {
            Duration --;
        } 
        
    }
