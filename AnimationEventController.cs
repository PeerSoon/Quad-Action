using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    public AudioSource moveAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource dodgeAudioSource;
    public AudioSource reloadAudioSource;

    public void PlayJumpSound()
    {
        jumpAudioSource.Play();
    }

    public void PlayDodgeSound()
    {
        dodgeAudioSource.Play();
    }

    public void PlayMoveSound()
    {
        moveAudioSource.Play();
    }

    public void PlayReloadSound()
    {
        reloadAudioSource.Play();
    }
}