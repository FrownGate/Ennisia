using UnityEngine;

public class Skill : MonoBehaviour
{
    protected int hitNb;
    protected float healingAmount;
    protected float damageAmount;
    protected float shieldAmount;
    protected float penDef;
    protected string description;
    protected string skillName;
    protected bool isAfter;
    protected bool use;
    protected bool isMagic;
    Texture2D texture;

    public virtual void Use(Entity target, Entity player)
    {
        

    }

}
