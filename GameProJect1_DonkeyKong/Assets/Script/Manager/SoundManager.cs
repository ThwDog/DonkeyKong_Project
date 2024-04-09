using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : SingletonClass<SoundManager>
{
    public Sound[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;   

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.Play();
    }

    public void PlayMusic(string nameSound)
    {
        Sound s = Array.Find(musicSound, x => x.nameSound == nameSound);
        if (s == null)
        {
            Debug.Log("Not have music");
        }
        else
        {
            Debug.Log($"Play music {nameSound}");
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void StopMusic(string nameSound)
    {
        Sound s = Array.Find(musicSound, x => x.nameSound == nameSound);
        if (s == null)
        {
            Debug.Log("Not have music");
        }
        else
        {
            Debug.Log($"Stop music {nameSound}");
            musicSource.clip = s.clip;
            musicSource.Stop();
        }
    }

    public void StopAllMusic()
    {
        Debug.Log("Stop all music");
        musicSource.Stop();
    }

    public void PlaySfx(string nameSfx)
    {
        Sound s = Array.Find(sfxSound, x => x.nameSound == nameSfx);
        if (s == null)
        {
            Debug.Log("Not have SFX");
        }
        else
        {
            Debug.Log($"Play Sfx {nameSfx}");
            sfxSource.PlayOneShot(s.clip);
        }
    }  

    public void StopSfx(string nameSfx)
    {
        Sound s = Array.Find(sfxSound, x => x.nameSound == nameSfx);
        if (s == null)
        {
            Debug.Log("Not have SFX");
        }
        else
        {
            Debug.Log($"Stop Sfx {nameSfx}");
            Debug.Log(s.clip);
            sfxSource.Stop();
        }
    }  

    public AudioClip SearchSfx(string nameSound)
    {
        AudioClip sound = null;
        Sound s = Array.Find(sfxSound, x => x.nameSound == nameSound);
        if (s == null)
        {
            Debug.Log("Not have SFX");
        }
        else
        {
            sound = s.clip;
        }
        return sound;
    }
}
