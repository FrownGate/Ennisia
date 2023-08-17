using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _SFXSlider;
    [SerializeField] private Toggle _FullScreenToggle;

    private void Awake()
    {
        _musicSlider.maxValue = 0.5f;
        _musicSlider.minValue = 0;
        _musicSlider.value = AudioManager.Instance.BGMSaveVolume;
        _SFXSlider.maxValue = 0.5f;
        _SFXSlider.minValue = 0;
        _SFXSlider.value = AudioManager.Instance.SFXSaveVolume;
    }

    private void Start()
    {
        _FullScreenToggle.isOn = Screen.fullScreen;

        _FullScreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
    }

    private void Update()
    {
        foreach (var sound in AudioManager.Instance.sounds.Where(sound => sound.name.Contains("Bgm")))
        {
            sound.Source.volume = _musicSlider.value;
            AudioManager.Instance.BGMSaveVolume = _musicSlider.value;
        }
        foreach (var sound in AudioManager.Instance.sounds.Where(sound => sound.name.Contains("SFX")))
        {
            sound.Source.volume = _SFXSlider.value;
            AudioManager.Instance.SFXSaveVolume = _SFXSlider.value;
        }
    }

    private void OnFullscreenToggleChanged(bool newValue)
    {
        Screen.fullScreen = newValue;
    }
}
