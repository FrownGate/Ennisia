using UnityEngine;

namespace Entities
{
    public class Enemy : Entity
    {

        public Enemy()
        {
            Damage = 20;
            MaxHp = 200;
            Level = 10;
            Speed = 200;
            CurrentHp = MaxHp / 2;
            
        }

        private void InitStats()
        {
            
        }
        
        
        
        
    }
}