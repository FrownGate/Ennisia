using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HolyPrayer : Skill
{
    // Start is called before the first frame update
    void Awake()
    {
        fileName = "HolyPrayer";
    }
    public virtual void Use(Entity target, Entity player, int turn)
    {
        //attacksBuff
        healingModifier = target.maxHp * 10 / 100;
    }

}
    
