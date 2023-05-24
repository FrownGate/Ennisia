using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonk : Skill
{
    /*Weapon weapon;*/

    private void Start()
    {
        damageAmount = 100;


        /*weapon = GetComponentInParent<Weapon>(true);
        isMagic = weapon.isMagic;*/
        

        description = "";
        name = "Bonk !";
    }
    public override void Use(Entity target,Entity player)
    {
        
    }
}
