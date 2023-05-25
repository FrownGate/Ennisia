using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonk : Skill
{
    

    private void Start()
    {
        data.damageAmount = 100;
        data.description = "";
        name = "Bonk !";
    }
    public override void Use(Entity target,Entity player, int turn)
    {
        
    }
}
