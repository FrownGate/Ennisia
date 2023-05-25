using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    /*Weapon weapon;*/
    protected int hitNb;
    protected float healingAmount;
    protected float damageAmount;
    protected float shieldAmount;
    protected float penDef;
    protected string description;
    protected string skillName;
    protected bool isAfter;
    protected bool AOE;
    protected bool use;
    protected bool isMagic;
    Texture2D texture;



    public virtual void Use(Entity target, Entity player, int turn)
    {
        

    }

}
