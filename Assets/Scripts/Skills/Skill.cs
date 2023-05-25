using UnityEditor;
using UnityEngine;

public class Skill : MonoBehaviour
{
    /*Weapon weapon;*/
    public SkillData data;
    public float damageModifier;
    public float cooldownn;
    public string fileName;

    private void Start()
    {
        data = AssetDatabase.LoadAssetAtPath<SkillData>(Application.dataPath + "/Skills/" + fileName + ".asset");

    }

    public virtual void Use(Entity target, Entity player,int turn)
    {
        

    }

}
