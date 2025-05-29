using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup masterMixerGroup;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        // Se asegura de que solo haya una instancia del AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Se crean los audiosources para cada sonido
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }
    }



    public void Play(string name)
    {
        // Se reproduce el sonido con el nombre indicado
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        // Se detiene el sonido con el nombre indicado
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        // Se detienen todos los sonidos
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }	

    public void RandomPitch(string name, float minPitch, float maxPitch)
    {
        // Se reproduce el sonido con un pitch aleatorio entre los valores indicados
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        s.source.Play();
    }

    public void SetMasterVolume(float volume)
    {
        // Se ajusta el volumen del master mixer
        masterMixerGroup.audioMixer.SetFloat("Master", volume);
    }
    public void SetMusicVolume(float volume)
    {
        // Se ajusta el volumen del music mixer
        masterMixerGroup.audioMixer.SetFloat("Music", volume);
    }
    public void SetSFXVolume(float volume)
    {
        // Se ajusta el volumen del SFX mixer
        masterMixerGroup.audioMixer.SetFloat("SFX", volume);
    }

    public float GetMasterVolume()
    {
        // Se obtiene el volumen del master mixer
        masterMixerGroup.audioMixer.GetFloat("Master", out float volume);
        return volume;
    }
    public float GetMusicVolume()
    {
        // Se obtiene el volumen del music mixer
        masterMixerGroup.audioMixer.GetFloat("Music", out float volume);
        return volume;
    }
    public float GetSFXVolume()
    {
        // Se obtiene el volumen del SFX mixer
        masterMixerGroup.audioMixer.GetFloat("SFX", out float volume);
        return volume;
    }
}
