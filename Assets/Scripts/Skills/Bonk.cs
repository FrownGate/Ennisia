using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonk : Skill
{
    

      
    public override float Use(List<Entity> target,Entity player, int turn)
    {
        data.damageAmount = 100;
        data.description = "";
        name = "Bonk !";
    }
    public override float Use(Entity target,Entity player, int turn)
    {
        return 0;
    }
}
