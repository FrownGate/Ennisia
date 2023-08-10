using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetsBonus : MonoBehaviour
{
    private static PetsBonus instance = null;
    public static PetsBonus Instance => instance;

    public int GoldBonus;
    public int AffinityBonus;
    public int GearUpdateChance;
    public int ControlPointBonus;
    public int AdditionalGearChance;

    public List<Pet> Pets;
    public List<int> Bonus;
    private PetSO[] _petList;

    public enum AllPets { CATTOW, FULFFY, KISEKI, NAOKI, OBISIDIAN, TANGERINE, YOICHI, YUKI };

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

        ;
        _petList = Resources.LoadAll<PetSO>("SO/Pets");

        foreach (PetSO file in _petList)
        {
            Type type = Type.GetType(CSVUtils.GetFileName(file.Name));
            Pets.Add((Pet)Activator.CreateInstance(type));
            UpdatePets();
        }
    }

    public void UpdatePets()
    {
        foreach(Pet item in Pets)
        {
            Bonus[Pets.IndexOf(item)] = item.Obatined ? item.BonusAmount : 0;
        }
        
    }




}
