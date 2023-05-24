using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonk : Skill
{
    

    private void Start()
    {
        damageAmount = 10
        description = "";
        name = "Bonk !";
    }
    public override void Use(Entity target,Entity player)
    {
        
    }
}
