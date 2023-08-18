using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _SFXSlider;
    [SerializeField] private Toggle _FullScreenToggle;
    private AudioMixer audioMixer;

    [SerializeField] private float MaxVolLvl;
    [SerializeField] private float MinVolLvl;

    private void Awake()
    {
        _musicSlider.maxValue = MaxVolLvl;
        _musicSlider.minValue = MinVolLvl;
        _SFXSlider.maxValue = MaxVolLvl;
        _SFXSlider.minValue = MinVolLvl;
        _FullScreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
        audioMixer = Resources.Load<AudioMixer>("Audio/AudioSettings");
        GetSettings();
    }

    private void GetSettings()
    {
        Debug.Log(PlayFabManager.Instance.Settings.MusicVolume);
        _musicSlider.value = PlayFabManager.Instance.Settings.MusicVolume;
        _SFXSlider.value = PlayFabManager.Instance.Settings.SFXVolume;
        _FullScreenToggle.isOn = PlayFabManager.Instance.Settings.Fullscreen;
    }

    public void SaveSettings()
    {
        Debug.Log(_musicSlider.value);
        PlayFabManager.Instance.Settings.MusicVolume = _musicSlider.value;
        PlayFabManager.Instance.Settings.SFXVolume = _SFXSlider.value;
        PlayFabManager.Instance.Settings.Fullscreen = _FullScreenToggle.isOn;
        Debug.Log(PlayFabManager.Instance.Settings.MusicVolume);
        PlayFabManager.Instance.UpdateData();
    }

    private void Update()
    {
        if (_musicSlider.value > -10)
        {
            audioMixer.SetFloat("BGMVolume", _musicSlider.value);
        }
        else
        {
            audioMixer.SetFloat("BGMVolume", -80);
        }

        if (_SFXSlider.value > -10)
        {
            audioMixer.SetFloat("SFXVolume", _SFXSlider.value);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", -80);
        }
    }

    private void OnFullscreenToggleChanged(bool newValue)
    {
        Screen.fullScreen = newValue;
    }
}