using System;

[Serializable]
public class SettingsData
{
    public float MusicVolume;
    public float SFXVolume;
    public bool Fullscreen;

    public SettingsData()
    {
        MusicVolume = 0.2f;
        SFXVolume = 0.2f;
        Fullscreen = false;
    }
}