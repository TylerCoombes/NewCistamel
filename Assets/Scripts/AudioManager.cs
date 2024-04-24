using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource Audio;
    public AudioClip[] audioClip;
    public PlayerController playerController;

    public bool isPlaying = false;
    

    public void Update()
    {
        if (!Audio.isPlaying)
        {
            isPlaying = false;
        }
    }

    // Update is called once per frame
    public void PlayRunningClip()
    {
        if(!isPlaying)
        {
            Audio.volume = 1f;
            Audio.clip = audioClip[0];
            Audio.Play();
        }
    }

    public void StopRunningClip()
    {
        if(Audio.clip == audioClip[0])
        {
            Audio.Stop();
        }
    }

    public void PlayPunchingClip()
    {
        if(playerController.canPunchSound == true)
        {
            isPlaying = true;
            Audio.volume = 0.8f;

            Audio.clip = audioClip[1];
            Audio.Play();
        }
    }

    public void PlayJumpClip()
    {
        Audio.time = 1f;
        Audio.volume = 1f;

        Audio.clip = audioClip[2];
        Audio.Play();
    }

    public void PlayGrappleClip()
    {
        isPlaying = true;
        Audio.volume = 0.6f;

        Audio.clip = audioClip[3];
        Audio.Play();
    }

    public void PlayPickupClip()
    {
        isPlaying = true;
        Audio.volume = 0.3f;

        Audio.clip = audioClip[4];
        Audio.Play();
    }

}
