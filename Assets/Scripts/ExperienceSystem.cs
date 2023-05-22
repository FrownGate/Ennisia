using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ExperienceSystem : MonoBehaviour
{
    public Image ProgressBar; // Référence à l'objet Image de la barre de progression
    public Text ExpText; // Référence au texte de l'expérience
    public Text LevelText; // Référence au texte du niveau

    private int _level = 1;
    private int _experience = 0;
    private Dictionary<int, int> _levelExperienceMap;

    private void Awake()
    {
        LoadLevelExperienceMap();
    }

    public void GainExperience()
    {
        int experienceToAdd = 5; // Montant d'expérience à ajouter lorsque le bouton est cliqué
        _experience += experienceToAdd;
        Debug.Log("Expérience actuelle : " + _experience);

        if (_levelExperienceMap.ContainsKey(_level) && _experience >= _levelExperienceMap[_level + 1])
        {
            _level++;
            _experience -= _levelExperienceMap[_level];
            Debug.Log("Niveau atteint : " + _level);
        }

        UpdateUI(); // Mettre à jour l'interface utilisateur après chaque gain d'expérience
    }

    private void LoadLevelExperienceMap()
    {
        _levelExperienceMap = new Dictionary<int, int>();

        // Chemin relatif vers le fichier CSV
        string filePath = Path.Combine(Application.dataPath, "CSV/PlayerXpCSVExport.csv");

        // Lecture du fichier CSV
        string[] csvLines = File.ReadAllLines(filePath);

        foreach (string line in csvLines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 2)
            {
                int level = int.Parse(values[0]);
                int experienceRequired = int.Parse(values[1]);

                _levelExperienceMap[level] = experienceRequired;
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + line);
            }
        }

        UpdateUI(); // Mettre à jour l'interface utilisateur après avoir chargé les données du fichier CSV
    }

    private void UpdateUI()
    {
        int experienceRequired = 0;

        if (_levelExperienceMap.ContainsKey(_level + 1))
        {
            experienceRequired = _levelExperienceMap[_level + 1];
        }

        ExpText.text = _experience + " / " + experienceRequired;
        LevelText.text = "Lvl: " + _level;

        float fillAmount = (float)_experience / experienceRequired;
        ProgressBar.fillAmount = fillAmount;
    }
}//egfgg
