using UnityEngine.Audio;
using UnityEngine;
using System;
class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    private float tempAudio;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.name = s.name;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
        }
        tempAudio = s.volume;
        s.source.volume = tempAudio;
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        tempAudio = s.source.volume;
        s.source.volume = 0;
    }
}
