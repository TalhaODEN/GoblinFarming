using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource playerFootstep;
    public static AudioManager audioManager;
    [SerializeField] private AudioSource wateringSound;
    private void Awake()
    {
        if(audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
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
    public void WateringSound(bool check)
    {
        if (check && !wateringSound.isPlaying)
        {
            wateringSound.Play();
        }
        else if (!check && wateringSound.isPlaying)
        {
            wateringSound.Stop();
        }
    }

}
