﻿namespace Entities
{
    public abstract class Entity
    {
        protected internal int Level{ get; protected set; }
        protected internal int MaxHp{ get; protected set; }
        protected internal int Damage{ get; protected set; }
        protected internal int Speed{ get; protected set; }
        protected internal int CurrentHp { get; set; }
        public bool IsSelected { get;  set; }

        public bool IsDead
        {
            get
            {
                bool isDead = CurrentHp <= 0 ? true : false;
                return isDead;
            }
            private set{}
        }
        
        public void TakeDamage(int damage)
        {
            CurrentHp -= damage;
            IsSelected = false;
        }

        public void HaveBeSelected()
        {
            IsSelected = true;
        }
    }
}