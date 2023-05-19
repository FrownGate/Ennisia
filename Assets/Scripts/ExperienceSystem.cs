using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ExperienceSystem : MonoBehaviour
{
    private int level = 1;
    private int experience = 0;
    private Dictionary<int, int> levelExperienceMap;

    private void Awake()
    {
        LoadLevelExperienceMap();
    }

    public void GainExperience()
    {
        int experienceToAdd = 5; // Montant d'expérience à ajouter lorsque le bouton est cliqué
        experience += experienceToAdd;
        Debug.Log("Expérience actuelle : " + experience);

        if (levelExperienceMap.ContainsKey(level) && experience >= levelExperienceMap[level+1])
        {
            level++;
            experience -= levelExperienceMap[level];
            Debug.Log("Niveau atteint : " + level);
        }
    }

    private void LoadLevelExperienceMap()
    {
        levelExperienceMap = new Dictionary<int, int>();

        // Chemin relatif vers le fichier CSV
        string filePath = Application.dataPath + "/CSV/PlayerXpCSVExport.csv";

        // Lecture du fichier CSV
        string[] csvLines = File.ReadAllLines(filePath);

        foreach (string line in csvLines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 2)
            {
                int level = int.Parse(values[0]);
                int experienceRequired = int.Parse(values[1]);

                levelExperienceMap[level] = experienceRequired;
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }
    }
}
