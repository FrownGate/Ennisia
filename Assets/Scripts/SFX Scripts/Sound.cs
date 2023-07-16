using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    /*[HideInInspector] */public AudioSource Source;
    public string name;
    public AudioClip clip;
    [Range(0,1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;
    public bool loop;
    public bool isBGM;
}
