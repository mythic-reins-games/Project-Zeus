using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static AudioSource soundPlayer;

    public static void PlaySound(string resourcePath)
    {
        if (resourcePath == "") return;
        if (soundPlayer == null)
        {
            soundPlayer = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();
        }
        AudioClip clip = (AudioClip)Resources.Load(resourcePath, typeof(AudioClip));
        soundPlayer.PlayOneShot(clip);
    }
}
