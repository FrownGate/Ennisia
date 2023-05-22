using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ExperienceSystem : MonoBehaviour
{
    public Image ProgressBar; // R�f�rence � l'objet Image de la barre de progression
    public Text ExpText; // R�f�rence au texte de l'exp�rience
    public Text LevelText; // R�f�rence au texte du niveau

    private int _level = 1;
    private int _experience = 0;
    private Dictionary<int, int> _levelExperienceMap;

    private void Awake()
    {
        LoadLevelExperienceMap();
    }

    public void GainExperience()
    {
        int experienceToAdd = 5; // Montant d'exp�rience � ajouter lorsque le bouton est cliqu�
        _experience += experienceToAdd;
        Debug.Log("Exp�rience actuelle : " + _experience);

        if (_levelExperienceMap.ContainsKey(_level) && _experience >= _levelExperienceMap[_level + 1])
        {
            _level++;
            _experience -= _levelExperienceMap[_level];
            Debug.Log("Niveau atteint : " + _level);
        }

        UpdateUI(); // Mettre � jour l'interface utilisateur apr�s chaque gain d'exp�rience
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

        UpdateUI(); // Mettre � jour l'interface utilisateur apr�s avoir charg� les donn�es du fichier CSV
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
