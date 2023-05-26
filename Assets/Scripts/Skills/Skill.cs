using System.Collections.Generic;
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
        data = AssetDatabase.LoadAssetAtPath<SkillData>("Assets/Resources/SO/Skills/" + fileName + ".asset");

    }
    public virtual void ConstantPassive(List<Entity> targets, Entity player, int turn)
    {


    }
    public virtual void PassiveBeforeAttack(List<Entity> targets, Entity player, int turn)
    {


    }

    public virtual float Use(List<Entity> targets, Entity player,int turn)
    {
        return 0;

    }

    public virtual float AdditionalDamage(List<Entity> targets, Entity player, int turn, float damage)
    {
        return 0;
    }

    public virtual void SkillAfterDamage(List<Entity> targets, Entity player, int turn, float damage)
    {

    }

    public virtual void PassiveAfterAttack(List<Entity> targets, Entity player, int turn, float damage)
    {

    }
}
