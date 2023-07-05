using System.Collections.Generic;
using UnityEngine;

public class AtkBarSystem
{
    public List<Entity> AllEntities;
   
    public AtkBarSystem(Entity player, List<Entity> enemies)
    {
        AllEntities = new List<Entity>();
        foreach (Entity ent in enemies) AllEntities.Add(ent);
        AllEntities.Add(player);
    }

    public void InitAtkBars()
    {
        Entity fastest = null;
        int fastestID = 0;

        foreach (Entity entity in AllEntities)
        {
            entity.AtkBarFillAmount = ((100 / AllEntities.Count) + (int)entity.Stats[Attribute.Speed].Value);

            if (fastest == null || entity.Stats[Attribute.Speed].Value > fastest.Stats[Attribute.Speed].Value)
            {
                fastest = entity;
                fastestID = AllEntities.IndexOf(entity);
            }
        }

        AllEntities[fastestID].AtkBar = 100;
        AllEntities[fastestID].AtkBarPercentage = 100;

        foreach (Entity entity in AllEntities)
        {
            entity.AtkBarPercentage = (int)(entity.Stats[Attribute.Speed].Value * 100) / (int)fastest.Stats[Attribute.Speed].Value;
            entity.AtkBar = entity.AtkBarPercentage;
        }
    }

    public void IncreaseAtkBars()
    {
        Entity fastest = null;
        int fastestID = 0;

        foreach (Entity entity in AllEntities)
        {
            entity.AtkBar += entity.AtkBarFillAmount;
            Debug.Log(entity.AtkBar);
        }

        foreach (Entity entity in AllEntities)
        {
            if (fastest == null || entity.AtkBar > fastest.AtkBar)
            {
                fastest = entity;
                fastestID = AllEntities.IndexOf(entity);
            }
        }

        AllEntities[fastestID].AtkBarPercentage = 100;

        foreach (Entity entity in AllEntities)
        {
            entity.AtkBarPercentage = (entity.AtkBar * 100) / fastest.AtkBar;
            Debug.Log(entity.Name + " atb " + entity.AtkBarPercentage);
        }
    }

}