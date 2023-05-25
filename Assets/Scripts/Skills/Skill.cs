using UnityEditor;
using UnityEngine;

public class Skill : MonoBehaviour
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

    public virtual float Use(Entity target, Entity player,int turn)
    {
        return 0;

    }

    public virtual float AdditionalDamage(Entity target, Entity player, int turn, float damage)
    {
        return 0;
    }

    public virtual void PassiveAfterAttack(Entity target, Entity player, int turn, float damage)
    {

    }
}
