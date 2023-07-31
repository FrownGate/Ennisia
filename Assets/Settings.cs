using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;

    private void Start()
    {
        _musicSlider.maxValue = 0.5f;
        _musicSlider.minValue = 0;
        _musicSlider.value = AudioManager.Instance.DefautlVolume;
    }

    private void Update()
    {
        foreach (var sound in AudioManager.Instance.sounds.Where(sound => sound.name.Contains("BGM")))
        {
            sound.Source.volume = _musicSlider.value;
        }
    }
} 
