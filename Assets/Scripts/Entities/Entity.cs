using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    // Change public to protected, once battle loop is done. 
  
    public string entityName;
    public int level;

    public int damage;

    public int maxHp;
    public int currentHp;
    public int speed;
    public bool isSelected { get; private set; } = false;

    public void OnMouseDown()
    {
        isSelected = true;
    }

    public bool IsDead
    {
        get
        {
            bool isDead = currentHp <= 0 ? true : false;
            return isDead;
        }
        private set{}
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        isSelected = false;
    }


}
