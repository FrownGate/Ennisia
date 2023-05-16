using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    public string entityName;
    public int level;

    public int damage;

    public int maxHp;
    public int battleHp;

    public bool TakeDamage(int damage)
    {
        battleHp -= damage;


        if(battleHp <= 0) 
        {
            battleHp= 0;
            return true;
        }
        else
        {
            return false;
        }
    }

}
