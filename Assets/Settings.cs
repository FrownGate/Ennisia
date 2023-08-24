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
        _SFXSlider.maxValue = 0.5f;
        _SFXSlider.minValue = 0;
        _FullScreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);

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