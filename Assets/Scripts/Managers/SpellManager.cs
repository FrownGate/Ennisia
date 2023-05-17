using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public struct Spell
    {
        public Spell(string name, int damage, int cd,int maxcd, int aoe)
        {
            NAME = name;
            DAMAGE = damage;
            COOLDOWN = cd;
            MAXCOOLDOWN = maxcd;
            AOECHECK = aoe;
        }
        public string NAME { get; }
        public int DAMAGE { get; }
        public int COOLDOWN { get; }
        public int MAXCOOLDOWN { get; }
        public int AOECHECK { get; }
    }

    public Spell spell1 = new Spell("Basic", 10, 0,0, 0);
    public Spell spell2 = new Spell("Tidal Wave", 5, 0, 2, 1);
    public Spell spell3 = new Spell("Blue dragon", 30, 0, 4, 0);
}
