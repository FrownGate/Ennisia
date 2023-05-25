using UnityEditor;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    /*Weapon weapon;*/
    public SkillData data;
    public float damageModifier;
    public float shieldModifier;
    public float healingModifier;
    public float cd;
    public string fileName;

    private void Start()
    {
        data = AssetDatabase.LoadAssetAtPath<SkillData>(Application.dataPath + "/Skills/SO/" + fileName + ".asset");

    }
    public virtual void ConstantPassive(Entity target, Entity player, int turn)
    {


    }
    public virtual void PassiveBeforeAttack(Entity target, Entity player, int turn)
    {


    }

    public virtual void Use(Entity target, Entity player,int turn)
    {
        

    }

    public virtual void AdditionalDamage(Entity target, Entity player, int turn)
    {

    }

    public virtual void PassiveAfterAttack(Entity target, Entity player, int turn)
    {

    }
}
