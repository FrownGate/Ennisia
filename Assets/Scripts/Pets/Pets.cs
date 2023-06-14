using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pets
{
    private PetSO _data;
    public string _name; //will be private
    private Sprite _icon;
    public int ActualXP;
    public int ToGetXp;
    public int AffinityLevel = 0;
    private bool _obtained = false;


    public Pets()
    {
        _data = Resources.Load<PetSO>("SO/Pets/" + GetType().Name);
        _name = _data.name;
        _icon = _data.Icon;
        Debug.Log(_data.name);
        ToGetXp = 100;
    }


    // Update is called once per frame
    public virtual void GetPet(Player player)
    {
        _obtained = true;
        //put trasformer on alterable stat of entity
    }

    public virtual void Play()
    {
        Debug.Log("Successfully played with" + _name);
        ActualXP += 50 + 20 * AffinityLevel;
        LevelUp();
    }

    public virtual void LevelUp()
    {
        if (ActualXP >= ToGetXp)
        {
            if (AffinityLevel < 10)
            {
        
                AffinityLevel++;
                ActualXP -= ToGetXp;
                ToGetXp *= 2;
                Debug.Log("Level up to level : " + AffinityLevel);
            }
            else
            {
                ActualXP = ToGetXp;
            }
        }

    }
}
