using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllBonus { EXP, REFUND, GOLD, AFFINITY, GEAREXP, UPGRADEGEAR, CONTROLPOINT, ADDITIONALGEAR };

public class PetsBonus : MonoBehaviour
{

    private static PetsBonus instance = null;
    public static PetsBonus Instance => instance;

    public List<Pet> Pets;
    private PetSO[] _petList;
    public Dictionary<AllBonus, int> Bonus;
    

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        _petList = Resources.LoadAll<PetSO>("SO/Pets");

        Bonus = InitDictionary();

        foreach (PetSO file in _petList)
        {
            Type type = Type.GetType(CSVUtils.GetFileName(file.Name));
            Pets.Add((Pet)Activator.CreateInstance(type));
            UpdatePets();
        }
    }


    protected Dictionary<AllBonus, int> InitDictionary()
    {
        Dictionary<AllBonus, int> dictionary = new();

        foreach (string bonus in Enum.GetNames(typeof(AllBonus)))
        {
            dictionary[Enum.Parse<AllBonus>(bonus)] = 1;
        }

        return dictionary;
    }
    // TO DO -> function to create according to db
    public void UpdatePets()
    {
        foreach(Pet item in Pets)
        {
            if (item.Obatined) { Bonus[item.Data.BonusType] += item.Data.BonusAmount; }
        }
        
    }




}
