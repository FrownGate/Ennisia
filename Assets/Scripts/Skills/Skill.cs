using UnityEditor;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    /*Weapon weapon;*/
    public SkillData data;
    public float damageModifier;
    public float shieldModifier;
    public float healingModifier;
    public float cooldownn;
    public string fileName;

    private void Start()
    {
        data = AssetDatabase.LoadAssetAtPath<SkillData>(Application.dataPath + "/Skills/SO" + fileName + ".asset");

    }

    public virtual void Use(Entity target, Entity player,int turn)
    {
        

    }

}
