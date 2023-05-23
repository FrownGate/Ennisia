using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ExperienceSystem : MonoBehaviour
{
    public Image ProgressBar;
    public Text ExpText;
    public Text LevelText;

    [SerializeField] private int experienceToAdd = 5; // Montant d'expérience à ajouter lorsque le bouton est cliqué

    private int _level = 1;
    private int _experience = 0;
    private Dictionary<int, int> _levelExperienceMap;

    private void Awake()
    {
        LoadLevelExperienceMap();
    }

    public void GainExperience()
    {
        _experience += experienceToAdd;
        Debug.Log("Expérience actuelle : " + _experience);

        while (_levelExperienceMap.ContainsKey(_level) && _experience >= _levelExperienceMap[_level + 1])
        {
            _level++;
            _experience -= _levelExperienceMap[_level];
            Debug.Log("Niveau atteint : " + _level);
        }

        UpdateUI();
    }

    private void LoadLevelExperienceMap()
    {
        _levelExperienceMap = new Dictionary<int, int>();

        string filePath = Path.Combine(Application.dataPath, "CSV/PlayerXpCSVExport.csv");

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

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_levelExperienceMap.ContainsKey(_level + 1))
        {
            int experienceRequired = _levelExperienceMap[_level + 1];
            ExpText.text = _experience + " / " + experienceRequired;
            LevelText.text = "Lvl: " + _level;

            float fillAmount = (float)_experience / experienceRequired;
            ProgressBar.fillAmount = fillAmount;
        }
        else
        {
            ExpText.text = "Max";
            LevelText.text = "Lvl: " + _level;
            ProgressBar.fillAmount = 1f;
        }
    }
}
