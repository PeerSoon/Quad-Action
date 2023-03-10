using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBackgroundMusic : MonoBehaviour
{
    public AudioSource backgroundMusic;

    void Awake()
    {
        MusicStart();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            backgroundMusic.loop = false;
            backgroundMusic.playOnAwake = false;
            backgroundMusic.Stop();
        }
    }
    void MusicStart()
    {
        backgroundMusic.Play();
        backgroundMusic.loop = true;
        backgroundMusic.playOnAwake = true;
    }
}
