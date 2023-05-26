using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidInstinct : Skill
{
    private void Awake()
    {
        fileName = "DruidInstinct";
    }
    private void Start()
    {
        
    }
    public override float Use(Entity target, Entity player, int turn)
    {

        return 0;
        
    }
}
