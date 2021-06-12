using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioSource bgm;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
        }
    }

    void Update()
    {
        if(BtnType.isSound == false)
        {
            bgm.mute = true;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        if(BtnType.isSound == true)
        {
            Sound s = Array.Find(sounds, Sound => Sound.name == name);
            s.source.PlayOneShot(s.source.clip);
        }        
    }
}
