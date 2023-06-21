using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (var sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();

            sound.Source.clip = sound.clip;
            sound.Source.volume = sound.volume;
            sound.Source.pitch = sound.pitch;
            sound.Source.loop = sound.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
     
        s.Source.Play();
    }
}
