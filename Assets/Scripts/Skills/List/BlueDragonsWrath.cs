using System.Collections.Generic;
using UnityEngine;

public class BlueDragonsWrath : Skill
{
    public override float Use(List<Entity> targets, Entity player, int turn)
    {
        float damage = Data.DamageAmount;
        targets[0].TakeDamage(damage);
        Cooldown = Data.MaxCooldown;
        //Boost 50% attack for 3 turns
        Debug.Log("Player's attack" + player.Stats[Item.AttributeStat.Attack].Value);
        new BuffEffect(3,player, entity =>
        {
            entity.AlterateStat(Item.AttributeStat.Attack, value => value * 1.5f, 1);
        } );
        Debug.Log("Player's attack" + player.Stats[Item.AttributeStat.Attack].Value);
        return damage;
    }
}