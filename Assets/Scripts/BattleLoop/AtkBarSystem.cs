using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkBarSystem
{

    public List<Entity> AllEntities;
   
    public AtkBarSystem(Entity player, List<Entity> enemies)
    {
        AllEntities = new List<Entity>();
        foreach (Entity ent in enemies)
        {
            AllEntities.Add(ent);
        }
        AllEntities.Add(player);
    }

    public void InitAtkBars()
    {
        Entity fastest = null;
        int fastestID = 0;
        foreach (Entity entity in AllEntities)
        {
            entity.atkBarFillAmount = ((100 / AllEntities.Count) + (int)entity.Stats[Item.AttributeStat.Speed].Value) ;
            if (fastest == null || entity.Stats[Item.AttributeStat.Speed].Value > fastest.Stats[Item.AttributeStat.Speed].Value)
            {
                fastest = entity;
                fastestID = AllEntities.IndexOf(entity);
            }
        }

        AllEntities[fastestID].atkBar = 100;
        AllEntities[fastestID].atkBarPercentage = 100;

        foreach (Entity entity in AllEntities)
        {
            entity.atkBarPercentage = (int)(entity.Stats[Item.AttributeStat.Speed].Value * 100) / (int)fastest.Stats[Item.AttributeStat.Speed].Value;
            entity.atkBar = entity.atkBarPercentage;
        }

    }

    public void IncreaseAtkBars()
    {
        Entity fastest = null;
        int fastestID = 0;
        foreach (Entity entity in AllEntities)
        {
            entity.atkBar += entity.atkBarFillAmount;
            Debug.Log(entity.atkBar);
        }
        foreach (Entity entity in AllEntities)
        {
            if (fastest == null || entity.atkBar > fastest.atkBar)
            {
                fastest = entity;
                fastestID = AllEntities.IndexOf(entity);
            }
        }

        AllEntities[fastestID].atkBarPercentage = 100;
        foreach (Entity entity in AllEntities)
        {
            entity.atkBarPercentage = (entity.atkBar * 100) / fastest.atkBar;
            Debug.Log(entity.Name + " atb " + entity.atkBarPercentage);
        }
    }

    public void ResetAtb(Entity entity)
    {
        entity.atkBar = 0;
        entity.atkBarPercentage = 0;
    }
}
