using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ExperienceSystem : MonoBehaviour
{
    private int _level = 1;
    private int _experience = 0;
    private Dictionary<int, int> _levelExperienceMap;

    private void Awake()
    {
        LoadLevelExperienceMap();
    }

    public void GainExperience()
    {
        int ExperienceToAdd = 5; // Montant d'expérience à ajouter lorsque le bouton est cliqué
        _experience += ExperienceToAdd;
        Debug.Log("Expérience actuelle : " + _experience);

        if (_levelExperienceMap.ContainsKey(_level) && _experience >= _levelExperienceMap[_level+1])
        {
            _level++;
            _experience -= _levelExperienceMap[_level];
            Debug.Log("Niveau atteint : " + _level);
        }
    }

    private void LoadLevelExperienceMap()
    {
        _levelExperienceMap = new Dictionary<int, int>();

        // Chemin relatif vers le fichier CSV
        string _filePath = Application.dataPath + "/CSV/PlayerXpCSVExport.csv";

        // Lecture du fichier CSV
        string[] _csvLines = File.ReadAllLines(_filePath);

        foreach (string _line in _csvLines)
        {
            string[] _values = _line.Split(',');

            if (_values.Length >= 2)
            {
                int _level = int.Parse(_values[0]);
                int _experienceRequired = int.Parse(_values[1]);

                _levelExperienceMap[_level] = _experienceRequired;
            }
            else
            {
                Debug.LogWarning("Format invalide dans le fichier CSV : " + _line);
            }
        }
    }
}
