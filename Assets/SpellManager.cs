using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public struct Spell
    {
        public Spell(string name, int damage, int cd,int maxcd, int aoe)
        {
            Name = name;
            Damage = damage;
            Cooldown = cd;
            MaxCooldown = maxcd;
            AOECheck = aoe;
        }

        public string Name { get; }
        public int Damage { get; }
        public int Cooldown { get; }
        public int MaxCooldown { get; }
        public int AOECheck { get; }
    }

    public Spell spell1 = new Spell("Basic", 10, 0,0, 0);
    public Spell spell2 = new Spell("Tidal Wave", 5, 0, 2, 1);
    public Spell spell3 = new Spell("Blue dragon", 30, 0, 4, 0);
}
