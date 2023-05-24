using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalCrash : Skill
{


    private void Start()
    {
        damageAmount = 100;
        description = "Deals 3 hits to all enemies, creating a huge tidal wave.";
        name = "Tidal Crash";
        AOE = true;
    }
    public override void Use(Entity target, Entity player)
    {

    }
}

