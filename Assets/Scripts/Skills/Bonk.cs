using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonk : Skill
{
    

    private void Start()
    {
        damageAmount = 100;
        description = "";
        name = "Bonk !";
    }
    public override void Use(Entity target,Entity player, int turn)
    {
        
    }
}
