using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    /*Weapon weapon;*/
    public SkillData data;
    public float damageModifier;
    public float cooldownn;
    
    public virtual void Use(Entity target, Entity player,int turn)
    {
        

    }

}
