using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiddingSFXController : MonoBehaviour
{
    public AudioClip startSkidAC;
    public AudioClip loopSkidAC;
    public AudioClip endSkidAC;

    public AudioSource skidAS;

    private bool LoopIsPlaying;
    private bool playLoop;

    [SerializeField] private PlayerCarController MyPCC;
    
    public void startSkidding()
    {
        skidAS.PlayOneShot(startSkidAC);
        skidAS.loop = true;
        skidAS.clip = loopSkidAC;
        playLoop = true;
    }

    private void Update()
    {
        skidAS.volume = MyPCC.SkidIntensity;
        if (!skidAS.isPlaying && playLoop && !LoopIsPlaying)
        {
            LoopIsPlaying = true;
            skidAS.Play();
        }
    }

    public void endSkidding()
    {
        playLoop = false;
        LoopIsPlaying = false;
        skidAS.loop = false;
        skidAS.PlayOneShot(endSkidAC);
    }
    
}
