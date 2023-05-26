using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPower : Skill
{
    private void Awake()
    {
        fileName = "RisingPower";
    }
    private void Start()
    {
        damageModifier = data.damageAmount;
    }
    public override float Use(List<Entity> target, Entity player, int turn)
    {
        //add weapon conndition, if two handed sword equiped -> increase atk by 30%, else increase atk by 15%
        return damageModifier = player.PhysAtk + player.PhysAtk*0.15f;
    }
}
