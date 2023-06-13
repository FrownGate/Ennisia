using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pets : MonoBehaviour
{
    private PetSO _data;
    private string _name;
    private Sprite _icon;
    private int _actualXP;
    private int _toGetXp;
    private int _affinityLevel = 0;
    private bool _obtained = false;


    public void Awake()
    {
        _data = Resources.Load<PetSO>("SO/Pets/" + GetType().Name);
    }


    // Update is called once per frame
    public virtual void GetPet(Player player)
    {
        _obtained = true;
        //put trasformer on alterable stat of entity
    }

    public virtual void Play()
    {
        _actualXP += 50;
        LevelUp();
    }

    public virtual void LevelUp()
    {
        if (_actualXP >= _toGetXp)
        {
            if (_affinityLevel < 10)
            {
                _affinityLevel++;
                _toGetXp *= 2;
            }
            else
            {
                _actualXP = _toGetXp;
            }
        }

    }
}
