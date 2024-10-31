using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource playerFootstep;
    public static AudioManager audioManager;

    private void Awake()
    {
        if(audioManager == null)
        {
            audioManager = this;
        }
    }
    public void Footsteps(bool moving)
    {
        if (moving)
        {
            if (!playerFootstep.isPlaying)
            {
                playerFootstep.Play(); 
            }
        }
        else
        {
            if (playerFootstep.isPlaying)
            {
                playerFootstep.Stop();
            }
        }
    }

}
