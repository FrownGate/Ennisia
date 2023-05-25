using UnityEngine;

namespace Entities
{
    public class Enemy : Entity
    {
        public  bool IsSelected { get; private set; } = false;

        public Enemy()
        {
            Damage = 20;
            MaxHp = 200;
            Level = 10;
            Speed = 200;
            CurrentHp = MaxHp / 2;
            
        }

        public void HaveBeSelected()
        {
            IsSelected = true;
        }

        public override void TakeDamage(int damage)
        {
            CurrentHp -= damage;
            IsSelected = false;
        }
        
        public override bool HaveBeTargeted()
        {
            if (this.IsSelected && !this.IsDead)
            {
                return true;
            }
            return false;
        }
        
    }
}